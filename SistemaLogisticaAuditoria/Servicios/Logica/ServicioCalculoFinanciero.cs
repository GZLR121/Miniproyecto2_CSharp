// Autores: Gael Zair Limón Rodríguez, Sebastián Vázquez Hernández y Ervin Axel Rocha Mireles
using System;
using SistemaLogisticaAuditoria.Dominio;

namespace SistemaLogisticaAuditoria.Servicios.Logica
{
    /// <summary>
    /// Clase de servicio de dominio independiente para el cálculo financiero escalonado (Reto 3 y RF4).
    /// Cumple con el estándar de nombres de Microsoft (_camelCase para campos privados, PascalCase para métodos).
    /// </summary>
    public class ServicioCalculoFinanciero
    {
        private ConfiguracionTarifas _configuracion;

        public ServicioCalculoFinanciero(ConfiguracionTarifas configuracion)
        {
            _configuracion = configuracion ?? throw new ArgumentNullException(nameof(configuracion));
        }

        public ConfiguracionTarifas Configuracion
        {
            get => _configuracion;
            set => _configuracion = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Reto 3: Algoritmo de Cálculo de Impuestos Escalonados y Tarifa Regional.
        /// - Si la zona es "ZONA A", el recargo es del 5% sobre el subtotal por peso;
        ///   si es "ZONA B", es del 10%; si es "ZONA C", del 18%.
        /// - Si el peso excede los 20 kg, cada kg extra cobra una penalización del 25% sobre el precio base JSON.
        /// - Si el valor declarado supera los $1,000.00, se suma un seguro obligatorio del 2% sobre dicho valor.
        /// - Se suma el costo fijo de envío, se calcula el IVA configurado en JSON y el total a pagar.
        /// </summary>
        public void CalcularDesglosePaquete(Paquete paquete)
        {
            if (paquete == null)
                throw new ArgumentNullException(nameof(paquete));

            // 1. Subtotal base por peso
            double subtotalPeso = Math.Round(paquete.PesoKg * _configuracion.PrecioBasePorKg, 2);

            // 2. Estructura condicional de selección múltiple para recargo regional por zona
            double porcentajeZona;
            switch (paquete.Zona)
            {
                case "ZONA A":
                    porcentajeZona = 0.05;
                    break;
                case "ZONA B":
                    porcentajeZona = 0.10;
                    break;
                case "ZONA C":
                    porcentajeZona = 0.18;
                    break;
                default:
                    throw new ArgumentException($"Zona geográfica no reconocida ('{paquete.Zona}'). Use Zona A, Zona B o Zona C.");
            }

            double recargoZona = Math.Round(subtotalPeso * porcentajeZona, 2);

            // 3. Evaluación condicional para penalización por sobrepeso (> 20 kg)
            double penalizacionSobrepeso = 0.0;
            if (paquete.PesoKg > _configuracion.PesoLimiteKg)
            {
                double kilosExcedentes = paquete.PesoKg - _configuracion.PesoLimiteKg;
                double costoPenalizacionPorKg = _configuracion.PrecioBasePorKg * _configuracion.PorcentajePenalizacionKgExtra;
                penalizacionSobrepeso = Math.Round(kilosExcedentes * costoPenalizacionPorKg, 2);
            }

            // 4. Evaluación condicional compuesta para seguro obligatorio (Valor Declarado > $1,000.00)
            double seguroValor = 0.0;
            if (paquete.ValorDeclarado > _configuracion.MontoMinimoSeguro && _configuracion.PorcentajeSeguroValor > 0)
            {
                seguroValor = Math.Round(paquete.ValorDeclarado * _configuracion.PorcentajeSeguroValor, 2);
            }

            // 5. Costo fijo de envío, Subtotal Neto, IVA y Total a Pagar
            double costoFijo = Math.Round(_configuracion.CostoFijoEnvio, 2);
            double subtotalNeto = Math.Round(subtotalPeso + recargoZona + penalizacionSobrepeso + seguroValor + costoFijo, 2);
            double montoIva = Math.Round(subtotalNeto * _configuracion.TasaIVA, 2);
            double totalPagar = Math.Round(subtotalNeto + montoIva, 2);

            paquete.SubtotalPeso = subtotalPeso;
            paquete.RecargoZona = recargoZona;
            paquete.PenalizacionSobrepeso = penalizacionSobrepeso;
            paquete.SeguroValorDeclarado = seguroValor;
            paquete.CostoFijoEnvio = costoFijo;
            paquete.SubtotalNeto = subtotalNeto;
            paquete.MontoIVA = montoIva;
            paquete.TotalPagar = totalPagar;
        }

        /// <summary>
        /// Recorre mediante un ciclo controlado por contador el arreglo de paquetes válidos
        /// para acumular los totales financieros del lote completo (RF4).
        /// </summary>
        public void CalcularTotalesLote(ResultadoProcesamientoLote resultado)
        {
            if (resultado == null)
                throw new ArgumentNullException(nameof(resultado));

            double sumaPeso = 0.0;
            double sumaSubPeso = 0.0;
            double sumaRecargos = 0.0;
            double sumaPenalizaciones = 0.0;
            double sumaSeguros = 0.0;
            double sumaSubNeto = 0.0;
            double sumaIva = 0.0;
            double granTotal = 0.0;

            Paquete[] arreglo = resultado.PaquetesValidos;
            for (int i = 0; i < arreglo.Length; i++)
            {
                Paquete p = arreglo[i];
                sumaPeso += p.PesoKg;
                sumaSubPeso += p.SubtotalPeso;
                sumaRecargos += p.RecargoZona;
                sumaPenalizaciones += p.PenalizacionSobrepeso;
                sumaSeguros += p.SeguroValorDeclarado;
                sumaSubNeto += p.SubtotalNeto;
                sumaIva += p.MontoIVA;
                granTotal += p.TotalPagar;
            }

            resultado.SumaPesoTotal = Math.Round(sumaPeso, 2);
            resultado.SumaSubtotalPeso = Math.Round(sumaSubPeso, 2);
            resultado.SumaRecargosZona = Math.Round(sumaRecargos, 2);
            resultado.SumaPenalizaciones = Math.Round(sumaPenalizaciones, 2);
            resultado.SumaSeguros = Math.Round(sumaSeguros, 2);
            resultado.SumaSubtotalNeto = Math.Round(sumaSubNeto, 2);
            resultado.SumaIVA = Math.Round(sumaIva, 2);
            resultado.GranTotalFacturado = Math.Round(granTotal, 2);
        }
    }
}
