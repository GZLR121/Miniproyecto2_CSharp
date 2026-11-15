// Autor: Gael Zair Limon Rdz
using System;
using System.Collections.Generic;
using System.Text;
using SistemaLogisticaAuditoria.Dominio;

namespace SistemaLogisticaAuditoria.Servicios.Logica
{
    /// <summary>
    /// Servicio encargado de gestionar dinámicamente los textos en Español e Inglés (RF2).
    /// </summary>
    public class GestorIdioma
    {
        private bool _esIngles;
        private readonly Dictionary<string, string> _es = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _en = new Dictionary<string, string>();

        public GestorIdioma()
        {
            InicializarDiccionarios();
        }

        public bool EsIngles
        {
            get => _esIngles;
            set => _esIngles = value;
        }

        public string ObtenerTexto(string clave)
        {
            var dic = _esIngles ? _en : _es;
            return dic.TryGetValue(clave, out string? valor) ? valor : clave;
        }

        public string FormatearInspeccionPaquete(Paquete p, ConfiguracionTarifas cfg)
        {
            StringBuilder sb = new StringBuilder();
            if (_esIngles)
            {
                sb.AppendLine("=== PACKAGE AUDIT & FINANCIAL BREAKDOWN ===");
                sb.AppendLine($"• Package Code:         {p.Codigo}");
                sb.AppendLine($"• Client Name:          {p.Cliente}");
                sb.AppendLine($"• Delivery Zone:        {p.Zona}");
                sb.AppendLine($"• Weight (kg):          {p.PesoKg:F2} kg");
                sb.AppendLine($"• Declared Value:       ${p.ValorDeclarado:N2}");
                sb.AppendLine($"• Transaction Status:   {p.EstadoTransaccion}");
                sb.AppendLine($"• Timestamp:            {p.FechaProcesamiento:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine("--------------------------------------------");
                sb.AppendLine($"1. Base Weight Subtotal (${cfg.PrecioBasePorKg:N2}/kg): ${p.SubtotalPeso:N2}");
                sb.AppendLine($"2. Regional Zone Surcharge ({p.Zona}):      ${p.RecargoZona:N2}");
                sb.AppendLine($"3. Overweight Penalty (> {cfg.PesoLimiteKg} kg):        ${p.PenalizacionSobrepeso:N2}");
                sb.AppendLine($"4. Mandatory Insurance (> ${cfg.MontoMinimoSeguro:N0}):    ${p.SeguroValorDeclarado:N2}");
                sb.AppendLine($"5. Fixed Shipping Fee:                  ${p.CostoFijoEnvio:N2}");
                sb.AppendLine("--------------------------------------------");
                sb.AppendLine($"• Net Subtotal:                         ${p.SubtotalNeto:N2}");
                sb.AppendLine($"• VAT Tax ({cfg.TasaIVA * 100:F0}%):                        ${p.MontoIVA:N2}");
                sb.AppendLine($">> TOTAL TO PAY:                        ${p.TotalPagar:N2}");
            }
            else
            {
                sb.AppendLine("=== AUDITORÍA E INSPECCIÓN DEL PAQUETE ===");
                sb.AppendLine($"• Código del Paquete:   {p.Codigo}");
                sb.AppendLine($"• Cliente:              {p.Cliente}");
                sb.AppendLine($"• Zona de Entrega:      {p.Zona}");
                sb.AppendLine($"• Peso Registrado:      {p.PesoKg:F2} kg");
                sb.AppendLine($"• Valor Declarado:      ${p.ValorDeclarado:N2}");
                sb.AppendLine($"• Estado en Base Datos: {p.EstadoTransaccion}");
                sb.AppendLine($"• Fecha de Registro:    {p.FechaProcesamiento:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine("--------------------------------------------");
                sb.AppendLine($"1. Subtotal por Peso (${cfg.PrecioBasePorKg:N2}/kg):    ${p.SubtotalPeso:N2}");
                sb.AppendLine($"2. Recargo por Zona ({p.Zona}):           ${p.RecargoZona:N2}");
                sb.AppendLine($"3. Penalización Sobrepeso (> {cfg.PesoLimiteKg} kg):    ${p.PenalizacionSobrepeso:N2}");
                sb.AppendLine($"4. Seguro Obligatorio (> ${cfg.MontoMinimoSeguro:N0}):       ${p.SeguroValorDeclarado:N2}");
                sb.AppendLine($"5. Costo Fijo de Envío:                 ${p.CostoFijoEnvio:N2}");
                sb.AppendLine("--------------------------------------------");
                sb.AppendLine($"• Subtotal Neto:                        ${p.SubtotalNeto:N2}");
                sb.AppendLine($"• Impuesto IVA ({cfg.TasaIVA * 100:F0}%):                   ${p.MontoIVA:N2}");
                sb.AppendLine($">> TOTAL A PAGAR:                       ${p.TotalPagar:N2}");
            }
            return sb.ToString();
        }

        private void InicializarDiccionarios()
        {
            _es["TituloVentana"] = "Sistema de Procesamiento y Auditoría de Facturación (Logística y Entregas)";
            _es["LblIdioma"] = "Idioma / Language:";
            _es["BtnCargarEjemplo"] = "Cargar Lote de Prueba";
            _es["GrpCaptura"] = "1. Captura de Lote de Paquetes (Formato: CODIGO;Cliente;Zona;PesoKg;ValorDeclarado)";
            _es["BtnProcesar"] = "Procesar Lote y Guardar en BD";
            _es["BtnInvertir"] = "Invertir Arreglo (Reto 2)";
            _es["BtnMarcarRecibido"] = "Registrar Recepción (BD)";
            _es["BtnExportarCsv"] = "Exportar a CSV (RF6)";
            _es["BtnHistorialBd"] = "Ver Transacciones BD";
            _es["GrpTabla"] = "2. Paquetes Validados (Ordenados de Mayor a Menor Costo Total - Algoritmo Burbuja)";
            _es["GrpBitacora"] = "3. Arreglo Paralelo de Errores Descartados (Reto 1) y Bitácora de Redimensionamiento (Reto 2)";
            _es["GrpInspeccion"] = "4. Panel Lateral de Inspección y Totales del Lote (RF5)";
            _es["ColCodigo"] = "Código";
            _es["ColCliente"] = "Cliente";
            _es["ColZona"] = "Zona";
            _es["ColPeso"] = "Peso (kg)";
            _es["ColValor"] = "Valor Decl.";
            _es["ColSubPeso"] = "Sub. Peso";
            _es["ColRecargo"] = "Rec. Zona";
            _es["ColPenal"] = "Penaliz.";
            _es["ColSeguro"] = "Seguro";
            _es["ColSubNeto"] = "Sub. Neto";
            _es["ColIVA"] = "IVA";
            _es["ColTotal"] = "Total a Pagar";
            _es["ColEstado"] = "Estado BD";
            _es["MsgJsonFaltante"] = "No se encontró el archivo 'appsettings.json'. Se generó e inicializó con las tarifas predeterminadas.";
            _es["MsgCsvExito"] = "El reporte de auditoría se exportó correctamente al archivo CSV:\n";
            _es["MsgSinPaquetes"] = "Primero debe procesar un lote con al menos un paquete válido.";
            _es["MsgSeleccioneFila"] = "Seleccione un paquete en la tabla para registrar su recepción en la base de datos.";
            _es["MsgRecepcionActualizada"] = "Transacción actualizada en Base de Datos como 'RECIBIDO EN DESTINO' para el paquete: ";

            _en["TituloVentana"] = "Billing Processing & Audit System (Express Logistics & Deliveries)";
            _en["LblIdioma"] = "Language / Idioma:";
            _en["BtnCargarEjemplo"] = "Load Sample Batch";
            _en["GrpCaptura"] = "1. Package Batch Input (Format: CODE;Client;Zone;WeightKg;DeclaredValue)";
            _en["BtnProcesar"] = "Process Batch & Save to DB";
            _en["BtnInvertir"] = "Reverse Array (Challenge 2)";
            _en["BtnMarcarRecibido"] = "Register Reception (DB)";
            _en["BtnExportarCsv"] = "Export to CSV (FR6)";
            _en["BtnHistorialBd"] = "View DB Transactions";
            _en["GrpTabla"] = "2. Validated Packages (Sorted Highest to Lowest Total Cost - Bubble Sort)";
            _en["GrpBitacora"] = "3. Parallel Discarded Error Array (Challenge 1) & Manual Array Resize Log (Challenge 2)";
            _en["GrpInspeccion"] = "4. Side Audit Inspection Panel & Batch Totals (FR5)";
            _en["ColCodigo"] = "Code";
            _en["ColCliente"] = "Client";
            _en["ColZona"] = "Zone";
            _en["ColPeso"] = "Weight (kg)";
            _en["ColValor"] = "Decl. Value";
            _en["ColSubPeso"] = "Weight Sub.";
            _en["ColRecargo"] = "Zone Sur.";
            _en["ColPenal"] = "Overweight";
            _en["ColSeguro"] = "Insurance";
            _en["ColSubNeto"] = "Net Sub.";
            _en["ColIVA"] = "VAT";
            _en["ColTotal"] = "Total Cost";
            _en["ColEstado"] = "DB Status";
            _en["MsgJsonFaltante"] = "The 'appsettings.json' file was not found. A default configuration file was created and loaded.";
            _en["MsgCsvExito"] = "The audit report was successfully exported to the CSV file:\n";
            _en["MsgSinPaquetes"] = "You must first process a batch containing at least one valid package.";
            _en["MsgSeleccioneFila"] = "Select a package row in the table to register its reception in the database.";
            _en["MsgRecepcionActualizada"] = "Database transaction updated to 'RECIBIDO EN DESTINO' for package: ";
        }
    }
}
