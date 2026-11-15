// Autor: Gael Zair Limon Rdz
using System;
using System.Globalization;
using System.IO;
using System.Text;
using SistemaLogisticaAuditoria.Dominio;

namespace SistemaLogisticaAuditoria.Servicios.Persistencia
{
    /// <summary>
    /// Servicio de persistencia para exportar el lote auditado a un archivo .csv delimitado por comas (RF6),
    /// utilizando descriptores de archivo envueltos en bloques de liberación de recursos (using).
    /// </summary>
    public class ServicioExportacionCsv
    {
        public void ExportarResultadoACsv(ResultadoProcesamientoLote resultado, string rutaDestinoCsv)
        {
            if (resultado == null)
                throw new ArgumentNullException(nameof(resultado));
            if (string.IsNullOrWhiteSpace(rutaDestinoCsv))
                throw new ArgumentException("La ruta de destino del archivo CSV no es válida.");

            using (FileStream flujoArchivo = new FileStream(rutaDestinoCsv, FileMode.Create, FileAccess.Write, FileShare.None))
            using (StreamWriter escritor = new StreamWriter(flujoArchivo, new UTF8Encoding(true)))
            {
                // Encabezado del archivo CSV
                escritor.WriteLine("Codigo,Cliente,Zona,PesoKg,ValorDeclarado,SubtotalPeso,RecargoZona,PenalizacionSobrepeso,SeguroValorDeclarado,CostoFijoEnvio,SubtotalNeto,MontoIVA,TotalPagar,EstadoTransaccion,FechaProcesamiento");

                Paquete[] paquetes = resultado.PaquetesValidos;
                for (int i = 0; i < paquetes.Length; i++)
                {
                    Paquete p = paquetes[i];
                    string clienteEscapado = EscaparCampoCsv(p.Cliente);
                    string linea = string.Format(
                        CultureInfo.InvariantCulture,
                        "{0},{1},{2},{3:F2},{4:F2},{5:F2},{6:F2},{7:F2},{8:F2},{9:F2},{10:F2},{11:F2},{12:F2},{13},{14:yyyy-MM-dd HH:mm:ss}",
                        p.Codigo,
                        clienteEscapado,
                        p.Zona,
                        p.PesoKg,
                        p.ValorDeclarado,
                        p.SubtotalPeso,
                        p.RecargoZona,
                        p.PenalizacionSobrepeso,
                        p.SeguroValorDeclarado,
                        p.CostoFijoEnvio,
                        p.SubtotalNeto,
                        p.MontoIVA,
                        p.TotalPagar,
                        p.EstadoTransaccion,
                        p.FechaProcesamiento);

                    escritor.WriteLine(linea);
                }

                // Línea de resumen financiero del lote
                escritor.WriteLine();
                escritor.WriteLine(string.Format(
                    CultureInfo.InvariantCulture,
                    "TOTALES_LOTE,Paquetes Validos: {0},Errores Descartados: {1},{2:F2},,{3:F2},{4:F2},{5:F2},{6:F2},,{7:F2},{8:F2},{9:F2},,",
                    paquetes.Length,
                    resultado.ErroresDetectados.Length,
                    resultado.SumaPesoTotal,
                    resultado.SumaSubtotalPeso,
                    resultado.SumaRecargosZona,
                    resultado.SumaPenalizaciones,
                    resultado.SumaSeguros,
                    resultado.SumaSubtotalNeto,
                    resultado.SumaIVA,
                    resultado.GranTotalFacturado));
            }
        }

        private static string EscaparCampoCsv(string valor)
        {
            if (string.IsNullOrEmpty(valor))
                return string.Empty;

            if (valor.Contains(',') || valor.Contains('"') || valor.Contains('\n'))
            {
                return "\"" + valor.Replace("\"", "\"\"") + "\"";
            }
            return valor;
        }
    }
}
