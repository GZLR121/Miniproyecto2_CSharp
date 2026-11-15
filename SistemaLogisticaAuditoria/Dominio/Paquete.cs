// Autor: Gael Zair Limon Rdz
using System;

namespace SistemaLogisticaAuditoria.Dominio
{
    /// <summary>
    /// Entidad de dominio que representa un paquete/factura de flete y su desglose financiero.
    /// Aplica encapsulamiento estricto con campos privados (_camelCase) y propiedades públicas (PascalCase).
    /// </summary>
    public class Paquete
    {
        private string _codigo = string.Empty;
        private string _cliente = string.Empty;
        private string _zona = string.Empty;
        private double _pesoKg;
        private double _valorDeclarado;
        private double _subtotalPeso;
        private double _recargoZona;
        private double _penalizacionSobrepeso;
        private double _seguroValorDeclarado;
        private double _costoFijoEnvio;
        private double _subtotalNeto;
        private double _montoIVA;
        private double _totalPagar;
        private string _estadoTransaccion = "ENVIADO";
        private DateTime _fechaProcesamiento = DateTime.Now;

        public Paquete(string codigo, string cliente, string zona, double pesoKg, double valorDeclarado)
        {
            Codigo = codigo;
            Cliente = cliente;
            Zona = zona;
            PesoKg = pesoKg;
            ValorDeclarado = valorDeclarado;
            _fechaProcesamiento = DateTime.Now;
        }

        public string Codigo
        {
            get => _codigo;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El código del paquete no puede estar vacío.");
                _codigo = value.Trim().ToUpperInvariant();
            }
        }

        public string Cliente
        {
            get => _cliente;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre del cliente no puede estar vacío.");
                _cliente = value.Trim();
            }
        }

        public string Zona
        {
            get => _zona;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La zona de envío no puede estar vacía.");
                _zona = value.Trim().ToUpperInvariant();
            }
        }

        public double PesoKg
        {
            get => _pesoKg;
            set
            {
                if (value <= 0)
                    throw new ArgumentException($"Peso inválido ({value} kg): el peso debe ser estrictamente positivo.");
                _pesoKg = value;
            }
        }

        public double ValorDeclarado
        {
            get => _valorDeclarado;
            set
            {
                if (value < 0)
                    throw new ArgumentException($"Valor declarado inválido (${value}): no puede ser negativo.");
                _valorDeclarado = value;
            }
        }

        public double SubtotalPeso
        {
            get => _subtotalPeso;
            set => _subtotalPeso = value;
        }

        public double RecargoZona
        {
            get => _recargoZona;
            set => _recargoZona = value;
        }

        public double PenalizacionSobrepeso
        {
            get => _penalizacionSobrepeso;
            set => _penalizacionSobrepeso = value;
        }

        public double SeguroValorDeclarado
        {
            get => _seguroValorDeclarado;
            set => _seguroValorDeclarado = value;
        }

        public double CostoFijoEnvio
        {
            get => _costoFijoEnvio;
            set => _costoFijoEnvio = value;
        }

        public double SubtotalNeto
        {
            get => _subtotalNeto;
            set => _subtotalNeto = value;
        }

        public double MontoIVA
        {
            get => _montoIVA;
            set => _montoIVA = value;
        }

        public double TotalPagar
        {
            get => _totalPagar;
            set => _totalPagar = value;
        }

        public string EstadoTransaccion
        {
            get => _estadoTransaccion;
            set => _estadoTransaccion = string.IsNullOrWhiteSpace(value) ? "ENVIADO" : value.Trim().ToUpperInvariant();
        }

        public DateTime FechaProcesamiento
        {
            get => _fechaProcesamiento;
            set => _fechaProcesamiento = value;
        }
    }
}
