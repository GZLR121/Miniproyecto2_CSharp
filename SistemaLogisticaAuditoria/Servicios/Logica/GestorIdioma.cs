// Autores: Gael Zair Limón Rodríguez, Sebastián Vázquez Hernández y Ervin Axel Rocha Mireles
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using SistemaLogisticaAuditoria.Dominio;

namespace SistemaLogisticaAuditoria.Servicios.Logica
{
    /**
     * La clase GestorIdioma permite cargar las etiquetas de la interfaz de usuario
     * desde el archivo JSON 'Recursos\etiquetas.json' (siguiendo el método visto en clase)
     * para cambiar dinámicamente entre Español, English, Français, Deutsch y Português (RF2).
     */
    public class GestorIdioma
    {
        private List<Etiquetas> _listaEtiquetas = new List<Etiquetas>();
        private Etiquetas _etiquetasSeleccionadas = new Etiquetas();
        private int _indiceLenguajeActual = 0;

        public GestorIdioma()
        {
            CargarArchivoEtiquetas();
        }

        public List<Etiquetas> ListaEtiquetas => _listaEtiquetas;

        public Etiquetas EtiquetasSeleccionadas => _etiquetasSeleccionadas;

        public int IndiceLenguajeActual
        {
            get => _indiceLenguajeActual;
            set => SeleccionarLenguaje(value);
        }

        public bool EsIngles
        {
            get => _indiceLenguajeActual == 1;
            set => SeleccionarLenguaje(value ? 1 : 0);
        }

        public void CargarArchivoEtiquetas()
        {
            string archivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Recursos", "etiquetas.json");
            if (!File.Exists(archivo))
            {
                archivo = "Recursos\\etiquetas.json";
            }

            if (File.Exists(archivo))
            {
                string jsonString = File.ReadAllText(archivo, Encoding.UTF8);
                List<Etiquetas>? deserializadas = JsonSerializer.Deserialize<List<Etiquetas>>(jsonString);
                if (deserializadas != null && deserializadas.Count > 0)
                {
                    _listaEtiquetas = deserializadas;
                    _etiquetasSeleccionadas = _listaEtiquetas[0];
                    _indiceLenguajeActual = 0;
                    return;
                }
            }

            _etiquetasSeleccionadas = new Etiquetas { Idioma = "Español", TituloVentana = "Sistema de Procesamiento y Auditoría de Facturación" };
            _listaEtiquetas = new List<Etiquetas> { _etiquetasSeleccionadas };
        }

        public Etiquetas SeleccionarLenguaje(int indiceLenguaje)
        {
            if (_listaEtiquetas.Count == 0)
            {
                CargarArchivoEtiquetas();
            }

            if (indiceLenguaje >= 0 && indiceLenguaje < _listaEtiquetas.Count)
            {
                _indiceLenguajeActual = indiceLenguaje;
                _etiquetasSeleccionadas = _listaEtiquetas[indiceLenguaje];
            }
            return _etiquetasSeleccionadas;
        }

        /**
         * Verifica si el texto de estado corresponde al estado de paquete ya recibido en cualquiera de los 5 idiomas.
         */
        public bool EsEstadoRecibido(string estadoActual)
        {
            if (string.IsNullOrWhiteSpace(estadoActual))
                return false;

            foreach (var et in _listaEtiquetas)
            {
                if (et.EstadosTransaccion != null && et.EstadosTransaccion.Count >= 3)
                {
                    if (string.Equals(estadoActual, et.EstadosTransaccion[2], StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }
            return estadoActual.Contains("RECIBIDO", StringComparison.OrdinalIgnoreCase)
                || estadoActual.Contains("RECEIVED", StringComparison.OrdinalIgnoreCase)
                || estadoActual.Contains("REÇU", StringComparison.OrdinalIgnoreCase)
                || estadoActual.Contains("EMPFANGEN", StringComparison.OrdinalIgnoreCase)
                || estadoActual.Contains("RECEBIDO", StringComparison.OrdinalIgnoreCase);
        }

        public string ObtenerEstadoTraducido(bool recibido)
        {
            if (_etiquetasSeleccionadas.EstadosTransaccion != null && _etiquetasSeleccionadas.EstadosTransaccion.Count >= 3)
            {
                return recibido ? _etiquetasSeleccionadas.EstadosTransaccion[2] : _etiquetasSeleccionadas.EstadosTransaccion[0];
            }
            return recibido ? "RECIBIDO EN DESTINO" : "ENVIADO (EN TRÁNSITO)";
        }

        public string ObtenerTexto(string clave)
        {
            Etiquetas e = _etiquetasSeleccionadas;
            return clave switch
            {
                "TituloVentana" => e.TituloVentana,
                "LblIdioma" => e.LblIdioma,
                "BtnCargarEjemplo" => e.BtnCargarEjemplo,
                "GrpCaptura" => e.GrpCaptura,
                "BtnProcesar" => e.BtnProcesar,
                "BtnInvertir" => e.BtnInvertir,
                "BtnMarcarRecibido" => e.BtnMarcarRecibido,
                "BtnExportarCsv" => e.BtnExportarCsv,
                "BtnHistorialBd" => e.BtnHistorialBd,
                "GrpTabla" => e.GrpTabla,
                "GrpBitacora" => e.GrpBitacora,
                "GrpInspeccion" => e.GrpInspeccion,
                "ColCodigo" => e.ColCodigo,
                "ColCliente" => e.ColCliente,
                "ColZona" => e.ColZona,
                "ColPeso" => e.ColPeso,
                "ColValor" => e.ColValor,
                "ColSubPeso" => e.ColSubPeso,
                "ColRecargo" => e.ColRecargo,
                "ColPenal" => e.ColPenal,
                "ColSeguro" => e.ColSeguro,
                "ColSubNeto" => e.ColSubNeto,
                "ColIVA" => e.ColIVA,
                "ColTotal" => e.ColTotal,
                "ColEstado" => e.ColEstado,
                "MsgJsonFaltante" => e.MsgJsonFaltante,
                "MsgCsvExito" => e.MsgCsvExito,
                "MsgSinPaquetes" => e.MsgSinPaquetes,
                "MsgSeleccioneFila" => e.MsgSeleccioneFila,
                "MsgRecepcionActualizada" => e.MsgRecepcionActualizada,
                _ => clave
            };
        }

        public string FormatearInspeccionPaquete(Paquete p, ConfiguracionTarifas cfg)
        {
            Etiquetas e = _etiquetasSeleccionadas;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(e.TituloInspeccion);
            sb.AppendLine($"• {e.ColCodigo}:               {p.Codigo}");
            sb.AppendLine($"• {e.ColCliente}:              {p.Cliente}");
            sb.AppendLine($"• {e.ColZona}:                 {p.Zona}");
            sb.AppendLine($"• {e.ColPeso}:            {p.PesoKg:F2} kg");
            sb.AppendLine($"• {e.ColValor}:          ${p.ValorDeclarado:N2}");
            sb.AppendLine($"• {e.ColEstado}:            {p.EstadoTransaccion}");
            sb.AppendLine($"• {e.EtiquetaFecha}:    {p.FechaProcesamiento:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine("--------------------------------------------");
            sb.AppendLine($"1. {e.ColSubPeso} (${cfg.PrecioBasePorKg:N2}/kg):   ${p.SubtotalPeso:N2}");
            sb.AppendLine($"2. {e.ColRecargo} ({p.Zona}):          ${p.RecargoZona:N2}");
            sb.AppendLine($"3. {e.ColPenal} (> {cfg.PesoLimiteKg} kg):        ${p.PenalizacionSobrepeso:N2}");
            sb.AppendLine($"4. {e.ColSeguro} (> ${cfg.MontoMinimoSeguro:N0}):         ${p.SeguroValorDeclarado:N2}");
            sb.AppendLine($"5. {e.EtiquetaEnvioFijo}:        ${p.CostoFijoEnvio:N2}");
            sb.AppendLine("--------------------------------------------");
            sb.AppendLine($"• {e.ColSubNeto}:                   ${p.SubtotalNeto:N2}");
            sb.AppendLine($"• {e.ColIVA} ({cfg.TasaIVA * 100:F0}%):                      ${p.MontoIVA:N2}");
            sb.AppendLine($">> {e.ColTotal.ToUpperInvariant()}:               ${p.TotalPagar:N2}");
            return sb.ToString();
        }

        public string FormatearResumenLote(ResultadoProcesamientoLote r)
        {
            Etiquetas e = _etiquetasSeleccionadas;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(e.TituloResumenLote);
            sb.AppendLine($"• {e.EtiquetaValidos}:    {r.PaquetesValidos.Length}  |  {e.EtiquetaErrores}: {r.ErroresDetectados.Length}");
            sb.AppendLine($"• {e.EtiquetaMatriz2D}:   string[{r.MatrizSegmentada.GetLength(0)}, {r.MatrizSegmentada.GetLength(1)}]");
            sb.AppendLine($"• {e.ColPeso}:          {r.SumaPesoTotal:F2} kg");
            sb.AppendLine($"• {e.ColSubNeto}:          ${r.SumaSubtotalNeto:N2}");
            sb.AppendLine($"• {e.ColIVA}:                ${r.SumaIVA:N2}");
            sb.AppendLine($">> {e.ColTotal.ToUpperInvariant()}:      ${r.GranTotalFacturado:N2}");
            return sb.ToString();
        }
    }
}