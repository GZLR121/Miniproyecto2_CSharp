// Autor: Gael Zair Limon Rdz
using System;

namespace SistemaLogisticaAuditoria.Dominio
{
    /// <summary>
    /// Modelo de dominio fuertemente tipado para los parámetros leídos desde appsettings.json (RF1).
    /// Cumple con el estándar de encapsulamiento (_camelCase para campos privados y PascalCase para propiedades).
    /// </summary>
    public class ConfiguracionTarifas
    {
        private double _precioBasePorKg = 25.00;
        private double _tasaIVA = 0.16;
        private double _costoFijoEnvio = 60.00;
        private double _pesoLimiteKg = 20.0;
        private double _porcentajePenalizacionKgExtra = 0.25;
        private double _montoMinimoSeguro = 1000.00;
        private double _porcentajeSeguroValor = 0.02;
        private string _rutaBaseDatos = "auditoria_logistica.db";

        public double PrecioBasePorKg
        {
            get => _precioBasePorKg;
            set => _precioBasePorKg = value > 0 ? value : throw new ArgumentException("El precio base por kg debe ser mayor a cero.");
        }

        public double TasaIVA
        {
            get => _tasaIVA;
            set => _tasaIVA = (value >= 0 && value <= 1.0) ? value : throw new ArgumentException("La tasa de IVA debe estar entre 0.00 y 1.00.");
        }

        public double CostoFijoEnvio
        {
            get => _costoFijoEnvio;
            set => _costoFijoEnvio = value >= 0 ? value : throw new ArgumentException("El costo fijo de envío no puede ser negativo.");
        }

        public double PesoLimiteKg
        {
            get => _pesoLimiteKg;
            set => _pesoLimiteKg = value > 0 ? value : 20.0;
        }

        public double PorcentajePenalizacionKgExtra
        {
            get => _porcentajePenalizacionKgExtra;
            set => _porcentajePenalizacionKgExtra = value >= 0 ? value : 0.25;
        }

        public double MontoMinimoSeguro
        {
            get => _montoMinimoSeguro;
            set => _montoMinimoSeguro = value >= 0 ? value : 1000.00;
        }

        public double PorcentajeSeguroValor
        {
            get => _porcentajeSeguroValor;
            set => _porcentajeSeguroValor = value >= 0 ? value : 0.02;
        }

        public string RutaBaseDatos
        {
            get => _rutaBaseDatos;
            set => _rutaBaseDatos = string.IsNullOrWhiteSpace(value) ? "auditoria_logistica.db" : value.Trim();
        }
    }
}
