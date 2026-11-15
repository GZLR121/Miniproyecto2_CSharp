// Autor: Gael Zair Limon Rdz
using System;
using System.Globalization;
using SistemaLogisticaAuditoria.Dominio;

namespace SistemaLogisticaAuditoria.Servicios.Logica
{
    /// <summary>
    /// Servicio encargado de:
    /// - Reto 1: Sanitización y Parseo de Texto con Tolerancia a Fallos (usando métodos de cadenas y arreglo paralelo de errores).
    /// - Reto 2: Redimensionamiento Dinámico Manual de Arreglos Estáticos (iniciando en capacidad 5 y duplicando capacidad),
    ///   Ordenamiento por Comparación (Burbuja de mayor a menor costo total) e Inversión manual de arreglos.
    /// </summary>
    public class ServicioProcesamientoLotes
    {
        public const int CapacidadInicialArreglo = 5;
        private readonly ServicioCalculoFinanciero _servicioFinanciero;

        public ServicioProcesamientoLotes(ServicioCalculoFinanciero servicioFinanciero)
        {
            _servicioFinanciero = servicioFinanciero ?? throw new ArgumentNullException(nameof(servicioFinanciero));
        }

        public ResultadoProcesamientoLote ProcesarTextoLote(string textoMultilinea, bool idiomaIngles = false)
        {
            if (string.IsNullOrWhiteSpace(textoMultilinea))
            {
                throw new ArgumentException(idiomaIngles
                    ? "The batch input text cannot be empty."
                    : "El texto del lote de entrada no puede estar vacío.");
            }

            // Normalización de saltos de línea y segmentación por líneas
            string textoNormalizado = textoMultilinea.Replace("\r\n", "\n").Replace("\r", "\n");
            string[] lineasCrudas = textoNormalizado.Split('\n');

            // Arreglos estáticos iniciales de tamaño fijo (Reto 2: capacidad inicial = 5)
            Paquete[] arregloBase = new Paquete[CapacidadInicialArreglo];
            int conteoValidos = 0;

            string[] arregloErrores = new string[CapacidadInicialArreglo];
            int conteoErrores = 0;

            string[] arregloBitacora = new string[CapacidadInicialArreglo];
            int conteoBitacora = 0;

            // Arreglo bidimensional string[,] para almacenar los metadatos segmentados de cada línea (Requisito de Arreglos 2D)
            string[,] matrizSegmentada = new string[lineasCrudas.Length, 5];

            RegistrarEnArregloTexto(ref arregloBitacora, ref conteoBitacora,
                idiomaIngles
                    ? $"[Init] Static array 'Paquete[] arregloBase' initialized with fixed capacity = {arregloBase.Length}."
                    : $"[Inicio] Arreglo estático 'Paquete[] arregloBase' inicializado con capacidad fija = {arregloBase.Length}.");

            // Recorrido línea por línea mediante ciclo controlado por contador (Reto 1)
            for (int numLinea = 0; numLinea < lineasCrudas.Length; numLinea++)
            {
                string lineaOriginal = lineasCrudas[numLinea];

                // Verificación de líneas vacías
                if (string.IsNullOrWhiteSpace(lineaOriginal))
                {
                    RegistrarEnArregloTexto(ref arregloErrores, ref conteoErrores,
                        idiomaIngles
                            ? $"Line {numLinea + 1}: Empty line ignored."
                            : $"Línea {numLinea + 1}: Línea vacía descartada.");
                    continue;
                }

                try
                {
                    // Operaciones de limpieza, reemplazo y segmentación de cadenas
                    string lineaSanitizada = SanitizarEspaciosMultiples(lineaOriginal.Trim());
                    string[] campos = lineaSanitizada.Split(';');

                    // Poblar la matriz bidimensional string[,] con los primeros 5 tokens
                    for (int col = 0; col < 5; col++)
                    {
                        matrizSegmentada[numLinea, col] = col < campos.Length ? campos[col].Trim() : string.Empty;
                    }

                    if (campos.Length != 5)
                    {
                        throw new FormatException(idiomaIngles
                            ? $"Invalid syntax ({campos.Length} fields found, 5 expected separated by ';')."
                            : $"Sintaxis inválida (se encontraron {campos.Length} campos, se esperaban 5 separados por ';').");
                    }

                    // Extracción y limpieza de cada campo con métodos de cadenas
                    string codigo = campos[0].Trim().ToUpperInvariant().Replace(" ", "");
                    string cliente = SanitizarEspaciosMultiples(campos[1].Trim());
                    string zona = SanitizarEspaciosMultiples(campos[2].Trim().ToUpperInvariant());
                    string pesoTexto = campos[3].Trim().Replace(",", ".");
                    string valorTexto = campos[4].Trim().Replace("$", "").Replace(",", ".");

                    // Validación léxica del código (ej. COD-101 o PACK-001)
                    if (!ValidarCodigoLexico(codigo))
                    {
                        throw new FormatException(idiomaIngles
                            ? $"Invalid package code '{codigo}' (expected format PREFIX-NNN, e.g. PACK-001 or COD-101)."
                            : $"Código de paquete inválido '{codigo}' (formato esperado PREFIJO-NNN, ej. PACK-001 o COD-101).");
                    }

                    if (string.IsNullOrWhiteSpace(cliente) || cliente.Length < 2)
                    {
                        throw new FormatException(idiomaIngles
                            ? "Client name is empty or too short."
                            : "El nombre del cliente está vacío o es demasiado corto.");
                    }

                    if (zona != "ZONA A" && zona != "ZONA B" && zona != "ZONA C")
                    {
                        throw new FormatException(idiomaIngles
                            ? $"Invalid zone '{zona}' (must be Zona A, Zona B, or Zona C)."
                            : $"Zona inválida '{zona}' (debe ser Zona A, Zona B o Zona C).");
                    }

                    // Validación numérica de peso y valor declarado
                    if (!double.TryParse(pesoTexto, NumberStyles.Float, CultureInfo.InvariantCulture, out double pesoKg))
                    {
                        throw new FormatException(idiomaIngles
                            ? $"Non-numeric weight format ('{campos[3].Trim()}')."
                            : $"Formato no numérico en el peso ('{campos[3].Trim()}').");
                    }

                    if (pesoKg <= 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(pesoKg), idiomaIngles
                            ? $"Negative or zero weight ({pesoKg} kg is not allowed)."
                            : $"Peso negativo o cero ({pesoKg} kg no permitido).");
                    }

                    if (!double.TryParse(valorTexto, NumberStyles.Float, CultureInfo.InvariantCulture, out double valorDeclarado))
                    {
                        throw new FormatException(idiomaIngles
                            ? $"Non-numeric declared value ('{campos[4].Trim()}')."
                            : $"Formato no numérico en el valor declarado ('{campos[4].Trim()}').");
                    }

                    if (valorDeclarado < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(valorDeclarado), idiomaIngles
                            ? $"Negative declared value (${valorDeclarado} is not allowed)."
                            : $"Valor declarado negativo (${valorDeclarado} no permitido).");
                    }

                    // Creación del objeto de dominio y cálculo financiero (Reto 3)
                    Paquete nuevoPaquete = new Paquete(codigo, cliente, zona, pesoKg, valorDeclarado);
                    _servicioFinanciero.CalcularDesglosePaquete(nuevoPaquete);

                    // Reto 2: Si el número de paquetes válidos alcanza la capacidad del arreglo estático,
                    // se duplica manualmente la capacidad copiando elemento por elemento.
                    if (conteoValidos >= arregloBase.Length)
                    {
                        int capacidadAnterior = arregloBase.Length;
                        arregloBase = RedimensionarArregloPaquetes(arregloBase, capacidadAnterior * 2);

                        RegistrarEnArregloTexto(ref arregloBitacora, ref conteoBitacora,
                            idiomaIngles
                                ? $"[Resize] Capacity exceeded at item #{conteoValidos + 1}: manual resize from {capacidadAnterior} to {arregloBase.Length} elements."
                                : $"[Redimensionamiento] Capacidad superada en elemento #{conteoValidos + 1}: arreglo duplicado manualmente de {capacidadAnterior} a {arregloBase.Length} posiciones.");
                    }

                    arregloBase[conteoValidos] = nuevoPaquete;
                    conteoValidos++;
                }
                catch (Exception ex)
                {
                    // Reto 1: Descartar la línea corrupta guardándola en el arreglo paralelo de errores sin detener el lote
                    string mensajeError = idiomaIngles
                        ? $"Line {numLinea + 1} discarded [{lineaOriginal.Trim()}] -> {ex.Message}"
                        : $"Línea {numLinea + 1} descartada [{lineaOriginal.Trim()}] -> {ex.Message}";

                    RegistrarEnArregloTexto(ref arregloErrores, ref conteoErrores, mensajeError);
                }
            }

            // Ajustar los arreglos a su conteo exacto copiando elemento por elemento
            Paquete[] paquetesFinales = AjustarTamanoExactoPaquetes(arregloBase, conteoValidos);

            // Reto 2: Ordenar el arreglo de paquetes de mayor a menor costo total (Algoritmo de la Burbuja)
            OrdenarPorCostoDescendenteBurbuja(paquetesFinales);

            RegistrarEnArregloTexto(ref arregloBitacora, ref conteoBitacora,
                idiomaIngles
                    ? $"[Sort] {paquetesFinales.Length} valid packages sorted descending by Total Cost using Bubble Sort."
                    : $"[Ordenamiento] {paquetesFinales.Length} paquetes válidos ordenados de mayor a menor Costo Total mediante Burbuja.");

            string[] erroresFinales = AjustarTamanoExactoTextos(arregloErrores, conteoErrores);
            string[] bitacoraFinal = AjustarTamanoExactoTextos(arregloBitacora, conteoBitacora);

            ResultadoProcesamientoLote resultado = new ResultadoProcesamientoLote
            {
                PaquetesValidos = paquetesFinales,
                ErroresDetectados = erroresFinales,
                MatrizSegmentada = matrizSegmentada,
                BitacoraArreglos = bitacoraFinal
            };

            _servicioFinanciero.CalcularTotalesLote(resultado);
            return resultado;
        }

        /// <summary>
        /// Reto 2: Algoritmo manual de redimensionamiento de arreglo tradicional copiando elemento por elemento.
        /// </summary>
        public static Paquete[] RedimensionarArregloPaquetes(Paquete[] origen, int nuevaCapacidad)
        {
            Paquete[] nuevoArreglo = new Paquete[nuevaCapacidad];
            int limite = origen.Length < nuevaCapacidad ? origen.Length : nuevaCapacidad;
            for (int i = 0; i < limite; i++)
            {
                nuevoArreglo[i] = origen[i];
            }
            return nuevoArreglo;
        }

        public static Paquete[] AjustarTamanoExactoPaquetes(Paquete[] origen, int cantidadElementos)
        {
            Paquete[] exacto = new Paquete[cantidadElementos];
            for (int i = 0; i < cantidadElementos; i++)
            {
                exacto[i] = origen[i];
            }
            return exacto;
        }

        /// <summary>
        /// Reto 2: Algoritmo de Ordenamiento por Comparación (Burbuja / Bubble Sort)
        /// que ordena los paquetes de MAYOR a MENOR costo total (TotalPagar).
        /// </summary>
        public static void OrdenarPorCostoDescendenteBurbuja(Paquete[] arreglo)
        {
            if (arreglo == null || arreglo.Length <= 1)
                return;

            int n = arreglo.Length;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (arreglo[j].TotalPagar < arreglo[j + 1].TotalPagar)
                    {
                        Paquete temp = arreglo[j];
                        arreglo[j] = arreglo[j + 1];
                        arreglo[j + 1] = temp;
                    }
                }
            }
        }

        /// <summary>
        /// Reto 2: Algoritmo manual de inversión de un arreglo tradicional (intercambiando extremos con índices i y n-1-i).
        /// </summary>
        public static void InvertirArregloPaquetes(Paquete[] arreglo)
        {
            if (arreglo == null || arreglo.Length <= 1)
                return;

            int izquierda = 0;
            int derecha = arreglo.Length - 1;
            while (izquierda < derecha)
            {
                Paquete temp = arreglo[izquierda];
                arreglo[izquierda] = arreglo[derecha];
                arreglo[derecha] = temp;
                izquierda++;
                derecha--;
            }
        }

        private static void RegistrarEnArregloTexto(ref string[] arreglo, ref int conteo, string elemento)
        {
            if (conteo >= arreglo.Length)
            {
                string[] nuevo = new string[arreglo.Length * 2];
                for (int i = 0; i < arreglo.Length; i++)
                {
                    nuevo[i] = arreglo[i];
                }
                arreglo = nuevo;
            }
            arreglo[conteo] = elemento;
            conteo++;
        }

        private static string[] AjustarTamanoExactoTextos(string[] origen, int cantidad)
        {
            string[] exacto = new string[cantidad];
            for (int i = 0; i < cantidad; i++)
            {
                exacto[i] = origen[i];
            }
            return exacto;
        }

        private static string SanitizarEspaciosMultiples(string texto)
        {
            while (texto.Contains("  "))
            {
                texto = texto.Replace("  ", " ");
            }
            return texto;
        }

        private static bool ValidarCodigoLexico(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo) || !codigo.Contains('-'))
                return false;

            string[] partes = codigo.Split('-');
            if (partes.Length != 2 || partes[0].Length < 2 || partes[1].Length < 1)
                return false;

            for (int i = 0; i < partes[0].Length; i++)
            {
                if (!char.IsLetterOrDigit(partes[0][i]))
                    return false;
            }

            for (int i = 0; i < partes[1].Length; i++)
            {
                if (!char.IsDigit(partes[1][i]))
                    return false;
            }

            return true;
        }
    }
}
