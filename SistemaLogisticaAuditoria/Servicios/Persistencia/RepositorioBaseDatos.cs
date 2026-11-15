// Autores: Gael Zair Limón Rodríguez, Sebastián Vázquez Hernández y Ervin Axel Rocha Mireles
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using SistemaLogisticaAuditoria.Dominio;

namespace SistemaLogisticaAuditoria.Servicios.Persistencia
{
    /// <summary>
    /// Repositorio de Base de Datos SQLite (usando el motor nativo de Windows winsqlite3.dll)
    /// para persistir las transacciones de envíos y recepciones de paquetes (RF4).
    /// También genera una bitácora SQL auditable (transacciones_sql.sql) en cada operación.
    /// </summary>
    public class RepositorioBaseDatos
    {
        private readonly string _rutaArchivoBd;
        private readonly string _rutaBitacoraSql;

        public RepositorioBaseDatos(string rutaArchivoBd)
        {
            _rutaArchivoBd = Path.IsPathRooted(rutaArchivoBd)
                ? rutaArchivoBd
                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rutaArchivoBd);

            _rutaBitacoraSql = Path.ChangeExtension(_rutaArchivoBd, ".sql");
            InicializarEsquemaBaseDatos();
        }

        public string RutaArchivoBd => _rutaArchivoBd;

        public void InicializarEsquemaBaseDatos()
        {
            string sqlCrearTabla = @"
CREATE TABLE IF NOT EXISTS TransaccionesEnviosRecepciones (
    IdTransaccion INTEGER PRIMARY KEY AUTOINCREMENT,
    CodigoPaquete TEXT NOT NULL,
    Cliente TEXT NOT NULL,
    Zona TEXT NOT NULL,
    PesoKg REAL NOT NULL,
    ValorDeclarado REAL NOT NULL,
    SubtotalPeso REAL NOT NULL,
    RecargoZona REAL NOT NULL,
    PenalizacionSobrepeso REAL NOT NULL,
    SeguroValorDeclarado REAL NOT NULL,
    CostoFijoEnvio REAL NOT NULL,
    SubtotalNeto REAL NOT NULL,
    MontoIVA REAL NOT NULL,
    TotalPagar REAL NOT NULL,
    EstadoTransaccion TEXT NOT NULL,
    FechaHoraRegistro TEXT NOT NULL
);";
            EjecutarComandoNoConsulta(sqlCrearTabla);
        }

        public void GuardarLoteEnBaseDatos(Paquete[] paquetes)
        {
            if (paquetes == null || paquetes.Length == 0)
                return;

            StringBuilder batchSql = new StringBuilder();
            batchSql.AppendLine("BEGIN TRANSACTION;");

            for (int i = 0; i < paquetes.Length; i++)
            {
                Paquete p = paquetes[i];
                string codigoSql = EscaparSql(p.Codigo);
                string clienteSql = EscaparSql(p.Cliente);
                string zonaSql = EscaparSql(p.Zona);
                string estadoSql = EscaparSql(p.EstadoTransaccion);
                string fechaSql = p.FechaProcesamiento.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                batchSql.AppendLine(string.Format(
                    CultureInfo.InvariantCulture,
                    "INSERT INTO TransaccionesEnviosRecepciones (CodigoPaquete, Cliente, Zona, PesoKg, ValorDeclarado, SubtotalPeso, RecargoZona, PenalizacionSobrepeso, SeguroValorDeclarado, CostoFijoEnvio, SubtotalNeto, MontoIVA, TotalPagar, EstadoTransaccion, FechaHoraRegistro) VALUES ('{0}', '{1}', '{2}', {3:F2}, {4:F2}, {5:F2}, {6:F2}, {7:F2}, {8:F2}, {9:F2}, {10:F2}, {11:F2}, {12:F2}, '{13}', '{14}');",
                    codigoSql, clienteSql, zonaSql,
                    p.PesoKg, p.ValorDeclarado, p.SubtotalPeso, p.RecargoZona,
                    p.PenalizacionSobrepeso, p.SeguroValorDeclarado, p.CostoFijoEnvio,
                    p.SubtotalNeto, p.MontoIVA, p.TotalPagar, estadoSql, fechaSql));
            }

            batchSql.AppendLine("COMMIT;");
            EjecutarComandoNoConsulta(batchSql.ToString());
        }

        public void ActualizarEstadoRecepcion(string codigoPaquete, string nuevoEstado)
        {
            if (string.IsNullOrWhiteSpace(codigoPaquete))
                return;

            string sql = string.Format(
                CultureInfo.InvariantCulture,
                "UPDATE TransaccionesEnviosRecepciones SET EstadoTransaccion = '{0}', FechaHoraRegistro = '{1}' WHERE CodigoPaquete = '{2}';",
                EscaparSql(nuevoEstado),
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                EscaparSql(codigoPaquete.Trim().ToUpperInvariant()));

            EjecutarComandoNoConsulta(sql);
        }

        public string[] ConsultarUltimasTransacciones(int limite = 50)
        {
            List<string> registros = new List<string>();
            IntPtr db = IntPtr.Zero;
            IntPtr stmt = IntPtr.Zero;

            try
            {
                if (NativeSqlite.sqlite3_open(_rutaArchivoBd, out db) != 0)
                    return registros.ToArray();

                string sql = $"SELECT IdTransaccion, CodigoPaquete, Cliente, Zona, PesoKg, TotalPagar, EstadoTransaccion, FechaHoraRegistro FROM TransaccionesEnviosRecepciones ORDER BY IdTransaccion DESC LIMIT {limite};";
                if (NativeSqlite.sqlite3_prepare_v2(db, sql, -1, out stmt, IntPtr.Zero) == 0)
                {
                    while (NativeSqlite.sqlite3_step(stmt) == 100) // SQLITE_ROW = 100
                    {
                        int id = NativeSqlite.sqlite3_column_int(stmt, 0);
                        string cod = NativeSqlite.ObtenerTextoColumna(stmt, 1);
                        string cli = NativeSqlite.ObtenerTextoColumna(stmt, 2);
                        string zon = NativeSqlite.ObtenerTextoColumna(stmt, 3);
                        double peso = NativeSqlite.sqlite3_column_double(stmt, 4);
                        double total = NativeSqlite.sqlite3_column_double(stmt, 5);
                        string est = NativeSqlite.ObtenerTextoColumna(stmt, 6);
                        string fec = NativeSqlite.ObtenerTextoColumna(stmt, 7);

                        registros.Add($"[ID #{id}] {fec} | {cod} | {cli} | {zon} | {peso:F2} kg | Total: ${total:N2} | Estado: {est}");
                    }
                }
            }
            catch
            {
                // Si hubiera algún bloqueo externo se retorna lo leído hasta el momento
            }
            finally
            {
                if (stmt != IntPtr.Zero)
                    NativeSqlite.sqlite3_finalize(stmt);
                if (db != IntPtr.Zero)
                    NativeSqlite.sqlite3_close(db);
            }

            return registros.ToArray();
        }

        private void EjecutarComandoNoConsulta(string sql)
        {
            // Guardar respaldo en archivo .sql auditable usando bloque using
            using (FileStream fs = new FileStream(_rutaBitacoraSql, FileMode.Append, FileAccess.Write, FileShare.Read))
            using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
            {
                sw.WriteLine($"-- Ejecutado: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                sw.WriteLine(sql.Trim());
                sw.WriteLine();
            }

            IntPtr db = IntPtr.Zero;
            try
            {
                if (NativeSqlite.sqlite3_open(_rutaArchivoBd, out db) == 0)
                {
                    NativeSqlite.sqlite3_exec(db, sql, IntPtr.Zero, IntPtr.Zero, out _);
                }
            }
            finally
            {
                if (db != IntPtr.Zero)
                    NativeSqlite.sqlite3_close(db);
            }
        }

        private static string EscaparSql(string valor)
        {
            return (valor ?? string.Empty).Replace("'", "''");
        }

        private static class NativeSqlite
        {
            private const string DllName = "winsqlite3.dll";

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
            public static extern int sqlite3_open([MarshalAs(UnmanagedType.LPUTF8Str)] string filename, out IntPtr db);

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern int sqlite3_close(IntPtr db);

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern int sqlite3_exec(
                IntPtr db,
                [MarshalAs(UnmanagedType.LPUTF8Str)] string sql,
                IntPtr callback,
                IntPtr arg,
                out IntPtr errMsg);

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern int sqlite3_prepare_v2(
                IntPtr db,
                [MarshalAs(UnmanagedType.LPUTF8Str)] string sql,
                int numBytes,
                out IntPtr stmt,
                IntPtr pzTail);

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern int sqlite3_step(IntPtr stmt);

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern int sqlite3_finalize(IntPtr stmt);

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern int sqlite3_column_int(IntPtr stmt, int iCol);

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern double sqlite3_column_double(IntPtr stmt, int iCol);

            [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
            public static extern IntPtr sqlite3_column_text(IntPtr stmt, int iCol);

            public static string ObtenerTextoColumna(IntPtr stmt, int iCol)
            {
                IntPtr ptr = sqlite3_column_text(stmt, iCol);
                return ptr == IntPtr.Zero ? string.Empty : Marshal.PtrToStringUTF8(ptr) ?? string.Empty;
            }
        }
    }
}
