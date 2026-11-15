// Autores: Gael Zair Limón Rodríguez, Sebastián Vázquez Hernández y Ervin Axel Rocha Mireles
using System;

namespace SistemaLogisticaAuditoria.Dominio
{
    /// <summary>
    /// Objeto de dominio que encapsula los arreglos nativos unidimensionales y bidimensionales
    /// resultantes del procesamiento de un lote de envíos, así como los acumulados financieros.
    /// </summary>
    public class ResultadoProcesamientoLote
    {
        private Paquete[] _paquetesValidos = Array.Empty<Paquete>();
        private string[] _erroresDetectados = Array.Empty<string>();
        private string[,] _matrizSegmentada = new string[0, 0];
        private string[] _bitacoraArreglos = Array.Empty<string>();
        private double _sumaPesoTotal;
        private double _sumaSubtotalPeso;
        private double _sumaRecargosZona;
        private double _sumaPenalizaciones;
        private double _sumaSeguros;
        private double _sumaSubtotalNeto;
        private double _sumaIVA;
        private double _granTotalFacturado;

        public Paquete[] PaquetesValidos
        {
            get => _paquetesValidos;
            set => _paquetesValidos = value ?? Array.Empty<Paquete>();
        }

        public string[] ErroresDetectados
        {
            get => _erroresDetectados;
            set => _erroresDetectados = value ?? Array.Empty<string>();
        }

        public string[,] MatrizSegmentada
        {
            get => _matrizSegmentada;
            set => _matrizSegmentada = value ?? new string[0, 0];
        }

        public string[] BitacoraArreglos
        {
            get => _bitacoraArreglos;
            set => _bitacoraArreglos = value ?? Array.Empty<string>();
        }

        public double SumaPesoTotal
        {
            get => _sumaPesoTotal;
            set => _sumaPesoTotal = value;
        }

        public double SumaSubtotalPeso
        {
            get => _sumaSubtotalPeso;
            set => _sumaSubtotalPeso = value;
        }

        public double SumaRecargosZona
        {
            get => _sumaRecargosZona;
            set => _sumaRecargosZona = value;
        }

        public double SumaPenalizaciones
        {
            get => _sumaPenalizaciones;
            set => _sumaPenalizaciones = value;
        }

        public double SumaSeguros
        {
            get => _sumaSeguros;
            set => _sumaSeguros = value;
        }

        public double SumaSubtotalNeto
        {
            get => _sumaSubtotalNeto;
            set => _sumaSubtotalNeto = value;
        }

        public double SumaIVA
        {
            get => _sumaIVA;
            set => _sumaIVA = value;
        }

        public double GranTotalFacturado
        {
            get => _granTotalFacturado;
            set => _granTotalFacturado = value;
        }
    }
}
