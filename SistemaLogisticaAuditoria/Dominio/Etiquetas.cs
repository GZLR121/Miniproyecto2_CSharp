// Autores: Gael Zair Limón Rodríguez, Sebastián Vázquez Hernández y Ervin Axel Rocha Mireles
using System;
using System.Collections.Generic;

namespace SistemaLogisticaAuditoria.Dominio
{
    /**
     * La clase Etiquetas representa las etiquetas de la interfaz de usuario
     * en diferentes idiomas leídas desde Recursos\etiquetas.json (RF2).
     * Aplica encapsulamiento estricto con campos privados (_camelCase) y propiedades públicas (PascalCase).
     */
    public class Etiquetas
    {
        private string _idioma = string.Empty;
        private string _tituloVentana = string.Empty;
        private string _lblIdioma = string.Empty;
        private string _btnCargarEjemplo = string.Empty;
        private string _grpCaptura = string.Empty;
        private string _btnProcesar = string.Empty;
        private string _btnInvertir = string.Empty;
        private string _btnMarcarRecibido = string.Empty;
        private string _btnExportarCsv = string.Empty;
        private string _btnHistorialBd = string.Empty;
        private string _grpTabla = string.Empty;
        private string _grpBitacora = string.Empty;
        private string _grpInspeccion = string.Empty;
        private string _colCodigo = string.Empty;
        private string _colCliente = string.Empty;
        private string _colZona = string.Empty;
        private string _colPeso = string.Empty;
        private string _colValor = string.Empty;
        private string _colSubPeso = string.Empty;
        private string _colRecargo = string.Empty;
        private string _colPenal = string.Empty;
        private string _colSeguro = string.Empty;
        private string _colSubNeto = string.Empty;
        private string _colIVA = string.Empty;
        private string _colTotal = string.Empty;
        private string _colEstado = string.Empty;
        private string _msgJsonFaltante = string.Empty;
        private string _msgCsvExito = string.Empty;
        private string _msgSinPaquetes = string.Empty;
        private string _msgSeleccioneFila = string.Empty;
        private string _msgRecepcionActualizada = string.Empty;
        private string _tituloInspeccion = string.Empty;
        private string _tituloResumenLote = string.Empty;
        private string _etiquetaFecha = string.Empty;
        private string _etiquetaEnvioFijo = string.Empty;
        private string _etiquetaValidos = string.Empty;
        private string _etiquetaErrores = string.Empty;
        private string _etiquetaMatriz2D = string.Empty;
        private List<string> _estadosTransaccion = new List<string>();

        public string Idioma { get => _idioma; set => _idioma = value ?? string.Empty; }
        public string TituloVentana { get => _tituloVentana; set => _tituloVentana = value ?? string.Empty; }
        public string LblIdioma { get => _lblIdioma; set => _lblIdioma = value ?? string.Empty; }
        public string BtnCargarEjemplo { get => _btnCargarEjemplo; set => _btnCargarEjemplo = value ?? string.Empty; }
        public string GrpCaptura { get => _grpCaptura; set => _grpCaptura = value ?? string.Empty; }
        public string BtnProcesar { get => _btnProcesar; set => _btnProcesar = value ?? string.Empty; }
        public string BtnInvertir { get => _btnInvertir; set => _btnInvertir = value ?? string.Empty; }
        public string BtnMarcarRecibido { get => _btnMarcarRecibido; set => _btnMarcarRecibido = value ?? string.Empty; }
        public string BtnExportarCsv { get => _btnExportarCsv; set => _btnExportarCsv = value ?? string.Empty; }
        public string BtnHistorialBd { get => _btnHistorialBd; set => _btnHistorialBd = value ?? string.Empty; }
        public string GrpTabla { get => _grpTabla; set => _grpTabla = value ?? string.Empty; }
        public string GrpBitacora { get => _grpBitacora; set => _grpBitacora = value ?? string.Empty; }
        public string GrpInspeccion { get => _grpInspeccion; set => _grpInspeccion = value ?? string.Empty; }
        public string ColCodigo { get => _colCodigo; set => _colCodigo = value ?? string.Empty; }
        public string ColCliente { get => _colCliente; set => _colCliente = value ?? string.Empty; }
        public string ColZona { get => _colZona; set => _colZona = value ?? string.Empty; }
        public string ColPeso { get => _colPeso; set => _colPeso = value ?? string.Empty; }
        public string ColValor { get => _colValor; set => _colValor = value ?? string.Empty; }
        public string ColSubPeso { get => _colSubPeso; set => _colSubPeso = value ?? string.Empty; }
        public string ColRecargo { get => _colRecargo; set => _colRecargo = value ?? string.Empty; }
        public string ColPenal { get => _colPenal; set => _colPenal = value ?? string.Empty; }
        public string ColSeguro { get => _colSeguro; set => _colSeguro = value ?? string.Empty; }
        public string ColSubNeto { get => _colSubNeto; set => _colSubNeto = value ?? string.Empty; }
        public string ColIVA { get => _colIVA; set => _colIVA = value ?? string.Empty; }
        public string ColTotal { get => _colTotal; set => _colTotal = value ?? string.Empty; }
        public string ColEstado { get => _colEstado; set => _colEstado = value ?? string.Empty; }
        public string MsgJsonFaltante { get => _msgJsonFaltante; set => _msgJsonFaltante = value ?? string.Empty; }
        public string MsgCsvExito { get => _msgCsvExito; set => _msgCsvExito = value ?? string.Empty; }
        public string MsgSinPaquetes { get => _msgSinPaquetes; set => _msgSinPaquetes = value ?? string.Empty; }
        public string MsgSeleccioneFila { get => _msgSeleccioneFila; set => _msgSeleccioneFila = value ?? string.Empty; }
        public string MsgRecepcionActualizada { get => _msgRecepcionActualizada; set => _msgRecepcionActualizada = value ?? string.Empty; }
        public string TituloInspeccion { get => _tituloInspeccion; set => _tituloInspeccion = value ?? string.Empty; }
        public string TituloResumenLote { get => _tituloResumenLote; set => _tituloResumenLote = value ?? string.Empty; }
        public string EtiquetaFecha { get => _etiquetaFecha; set => _etiquetaFecha = value ?? string.Empty; }
        public string EtiquetaEnvioFijo { get => _etiquetaEnvioFijo; set => _etiquetaEnvioFijo = value ?? string.Empty; }
        public string EtiquetaValidos { get => _etiquetaValidos; set => _etiquetaValidos = value ?? string.Empty; }
        public string EtiquetaErrores { get => _etiquetaErrores; set => _etiquetaErrores = value ?? string.Empty; }
        public string EtiquetaMatriz2D { get => _etiquetaMatriz2D; set => _etiquetaMatriz2D = value ?? string.Empty; }
        public List<string> EstadosTransaccion { get => _estadosTransaccion; set => _estadosTransaccion = value ?? new List<string>(); }
    }
}