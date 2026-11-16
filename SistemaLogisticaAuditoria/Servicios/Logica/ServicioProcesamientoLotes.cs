// Autores: Gael Zair Limón Rodríguez, Sebastián Vázquez Hernández y Ervin Axel Rocha Mireles
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

        public ResultadoProcesamientoLote ProcesarTextoLote(string textoMultilinea, bool idiomaIngles)
        {
            return ProcesarTextoLote(textoMultilinea, idiomaIngles ? 1 : 0);
        }

        public ResultadoProcesamientoLote ProcesarTextoLote(string textoMultilinea, int indiceLenguaje = 0)
        {
            if (string.IsNullOrWhiteSpace(textoMultilinea))
            {
                throw new ArgumentException(indiceLenguaje switch
                {
                    1 => "The batch input text cannot be empty.",
                    2 => "Le texte du lot d'entrée ne peut pas être vide.",
                    3 => "Der Eingabetext der Charge darf nicht leer sein.",
                    4 => "O texto do lote de entrada não pode estar vazio.",
                    _ => "El texto del lote de entrada no puede estar vacío."
                });
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
                indiceLenguaje switch
                {
                    1 => $"[Init] Static array 'Paquete[] arregloBase' initialized with fixed capacity = {arregloBase.Length}.",
                    2 => $"[Début] Tableau statique 'Paquete[] arregloBase' initialisé avec capacité fixe = {arregloBase.Length}.",
                    3 => $"[Start] Statisches Array 'Paquete[] arregloBase' initialisiert mit fester Kapazität = {arregloBase.Length}.",
                    4 => $"[Início] Vetor estático 'Paquete[] arregloBase' inicializado com capacidade fixa = {arregloBase.Length}.",
                    _ => $"[Inicio] Arreglo estático 'Paquete[] arregloBase' inicializado con capacidad fija = {arregloBase.Length}."
                });

            // Recorrido línea por línea mediante ciclo controlado por contador (Reto 1)
            for (int numLinea = 0; numLinea < lineasCrudas.Length; numLinea++)
            {
                string lineaOriginal = lineasCrudas[numLinea];

                // Verificación de líneas vacías
                if (string.IsNullOrWhiteSpace(lineaOriginal))
                {
                    RegistrarEnArregloTexto(ref arregloErrores, ref conteoErrores,
                        indiceLenguaje switch
                        {
                            1 => $"Line {numLinea + 1}: Empty line ignored.",
                            2 => $"Ligne {numLinea + 1} : Ligne vide ignorée.",
                            3 => $"Zeile {numLinea + 1}: Leere Zeile verworfen.",
                            4 => $"Linha {numLinea + 1}: Linha vazia descartada.",
                            _ => $"Línea {numLinea + 1}: Línea vacía descartada."
                        });
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
                        throw new FormatException(indiceLenguaje switch
                        {
                            1 => $"Invalid syntax ({campos.Length} fields found, 5 expected separated by ';').",
                            2 => $"Syntaxe invalide ({campos.Length} champs trouvés, 5 attendus séparés par ';').",
                            3 => $"Ungültige Syntax ({campos.Length} Felder gefunden, 5 getrennt durch ';' erwartet).",
                            4 => $"Sintaxe inválida ({campos.Length} campos encontrados, esperados 5 separados por ';').",
                            _ => $"Sintaxis inválida (se encontraron {campos.Length} campos, se esperaban 5 separados por ';')."
                        });
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
                        throw new FormatException(indiceLenguaje switch
                        {
                            1 => $"Invalid package code '{codigo}' (expected format PREFIX-NNN, e.g. PACK-001 or COD-101).",
                            2 => $"Code de colis invalide '{codigo}' (format attendu PREFIXE-NNN, ex. PACK-001 ou COD-101).",
                            3 => $"Ungültiger Paketcode '{codigo}' (erwartetes Format PRÄFIX-NNN, z.B. PACK-001 oder COD-101).",
                            4 => $"Código de encomenda inválido '{codigo}' (formato esperado PREFIXO-NNN, ex. PACK-001 ou COD-101).",
                            _ => $"Código de paquete inválido '{codigo}' (formato esperado PREFIJO-NNN, ej. PACK-001 o COD-101)."
                        });
                    }

                    if (string.IsNullOrWhiteSpace(cliente) || cliente.Length < 2)
                    {
                        throw new FormatException(indiceLenguaje switch
                        {
                            1 => "Client name is empty or too short.",
                            2 => "Le nom du destinataire est vide ou trop court.",
                            3 => "Der Kundenname ist leer oder zu kurz.",
                            4 => "O nome do destinatário está vazio ou é muito curto.",
                            _ => "El nombre del cliente está vacío o es demasiado corto."
                        });
                    }

                    if (zona != "ZONA A" && zona != "ZONA B" && zona != "ZONA C")
                    {
                        throw new FormatException(indiceLenguaje switch
                        {
                            1 => $"Invalid zone '{zona}' (must be Zona A, Zona B, or Zona C).",
                            2 => $"Secteur invalide '{zona}' (doit être Zona A, Zona B ou Zona C).",
                            3 => $"Ungültiges Gebiet '{zona}' (muss Zona A, Zona B oder Zona C sein).",
                            4 => $"Região inválida '{zona}' (deve ser Zona A, Zona B ou Zona C).",
                            _ => $"Zona inválida '{zona}' (debe ser Zona A, Zona B o Zona C)."
                        });
                    }

                    // Validación numérica de peso y valor declarado
                    if (!double.TryParse(pesoTexto, NumberStyles.Float, CultureInfo.InvariantCulture, out double pesoKg))
                    {
                        throw new FormatException(indiceLenguaje switch
                        {
                            1 => $"Non-numeric weight format ('{campos[3].Trim()}').",
                            2 => $"Format de poids non numérique ('{campos[3].Trim()}').",
                            3 => $"Nicht-numerisches Gewichtsformat ('{campos[3].Trim()}').",
                            4 => $"Formato não numérico na massa/peso ('{campos[3].Trim()}').",
                            _ => $"Formato no numérico en el peso ('{campos[3].Trim()}')."
                        });
                    }

                    if (pesoKg <= 0)
                    {
                        throw new FormatException(indiceLenguaje switch
                        {
                            1 => $"Negative or zero weight ({pesoKg} kg is not allowed).",
                            2 => $"Poids négatif ou nul ({pesoKg} kg non autorisé).",
                            3 => $"Negatives oder Null-Gewicht ({pesoKg} kg nicht zulässig).",
                            4 => $"Massa/peso negativo ou zero ({pesoKg} kg não permitido).",
                            _ => $"Peso negativo o cero ({pesoKg} kg no permitido)."
                        });
                    }

                    if (!double.TryParse(valorTexto, NumberStyles.Float, CultureInfo.InvariantCulture, out double valorDeclarado))
                    {
                        throw new FormatException(indiceLenguaje switch
                        {
                            1 => $"Non-numeric declared value ('{campos[4].Trim()}').",
                            2 => $"Valeur déclarée non numérique ('{campos[4].Trim()}').",
                            3 => $"Nicht-numerischer deklarierter Wert ('{campos[4].Trim()}').",
                            4 => $"Formato não numérico no valor declarado ('{campos[4].Trim()}').",
                            _ => $"Formato no numérico en el valor declarado ('{campos[4].Trim()}')."
                        });
                    }

                    if (valorDeclarado < 0)
                    {
                        throw new FormatException(indiceLenguaje switch
                        {
                            1 => $"Negative declared value (${valorDeclarado} is not allowed).",
                            2 => $"Valeur déclarée négative (${valorDeclarado} non autorisée).",
                            3 => $"Negativer deklarierter Wert (${valorDeclarado} nicht zulässig).",
                            4 => $"Valor declarado negativo (${valorDeclarado} não permitido).",
                            _ => $"Valor declarado negativo (${valorDeclarado} no permitido)."
                        });
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
                            indiceLenguaje switch
                            {
                                1 => $"[Resize] Capacity exceeded at item #{conteoValidos + 1}: manual resize from {capacidadAnterior} to {arregloBase.Length} elements.",
                                2 => $"[Redimensionnement] Capacité dépassée à l'élément #{conteoValidos + 1} : tableau doublé manuellement de {capacidadAnterior} à {arregloBase.Length} positions.",
                                3 => $"[Größenänderung] Kapazität überschritten bei Element #{conteoValidos + 1}: manuelle Verdopplung von {capacidadAnterior} auf {arregloBase.Length} Elemente.",
                                4 => $"[Redimensionamento] Capacidade excedida no item #{conteoValidos + 1}: vetor duplicado manualmente de {capacidadAnterior} para {arregloBase.Length} posições.",
                                _ => $"[Redimensionamiento] Capacidad superada en elemento #{conteoValidos + 1}: arreglo duplicado manualmente de {capacidadAnterior} a {arregloBase.Length} posiciones."
                            });
                    }

                    arregloBase[conteoValidos] = nuevoPaquete;
                    conteoValidos++;
                }
                catch (Exception ex)
                {
                    // Reto 1: Descartar la línea corrupta guardándola en el arreglo paralelo de errores sin detener el lote
                    string mensajeError = indiceLenguaje switch
                    {
                        1 => $"Line {numLinea + 1} discarded [{lineaOriginal.Trim()}] -> {ex.Message}",
                        2 => $"Ligne {numLinea + 1} rejetée [{lineaOriginal.Trim()}] -> {ex.Message}",
                        3 => $"Zeile {numLinea + 1} verworfen [{lineaOriginal.Trim()}] -> {ex.Message}",
                        4 => $"Linha {numLinea + 1} rejeitada [{lineaOriginal.Trim()}] -> {ex.Message}",
                        _ => $"Línea {numLinea + 1} descartada [{lineaOriginal.Trim()}] -> {ex.Message}"
                    };

                    RegistrarEnArregloTexto(ref arregloErrores, ref conteoErrores, mensajeError);
                }
            }

            // Ajustar los arreglos a su conteo exacto copiando elemento por elemento
            Paquete[] paquetesFinales = AjustarTamanoExactoPaquetes(arregloBase, conteoValidos);

            // Reto 2: Ordenar el arreglo de paquetes de mayor a menor costo total (Algoritmo de la Burbuja)
            OrdenarPorCostoDescendenteBurbuja(paquetesFinales);

            RegistrarEnArregloTexto(ref arregloBitacora, ref conteoBitacora,
                indiceLenguaje switch
                {
                    1 => $"[Sort] {paquetesFinales.Length} valid packages sorted descending by Total Cost using Bubble Sort.",
                    2 => $"[Tri] {paquetesFinales.Length} colis valides triés par coût total décroissant via Tri à Bulles.",
                    3 => $"[Sortierung] {paquetesFinales.Length} gültige Pakete absteigend nach Gesamtkosten mit Bubblesort sortiert.",
                    4 => $"[Ordenação] {paquetesFinales.Length} encomendas válidas ordenadas do maior ao menor custo via Método Bolha.",
                    _ => $"[Ordenamiento] {paquetesFinales.Length} paquetes válidos ordenados de mayor a menor Costo Total mediante Burbuja."
                });

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

        private static bool ValidarCodigoLexico(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo) || codigo.Length < 4)
                return false;

            int indiceGuion = codigo.IndexOf('-');
            if (indiceGuion <= 0 || indiceGuion >= codigo.Length - 1)
                return false;

            string prefijo = codigo.Substring(0, indiceGuion);
            string sufijoNumerico = codigo.Substring(indiceGuion + 1);

            for (int i = 0; i < prefijo.Length; i++)
            {
                if (!char.IsLetter(prefijo[i]))
                    return false;
            }

            for (int i = 0; i < sufijoNumerico.Length; i++)
            {
                if (!char.IsDigit(sufijoNumerico[i]))
                    return false;
            }

            return true;
        }

        private static string SanitizarEspaciosMultiples(string entrada)
        {
            if (string.IsNullOrEmpty(entrada))
                return string.Empty;

            string resultado = entrada.Trim();
            while (resultado.Contains("  "))
            {
                resultado = resultado.Replace("  ", " ");
            }
            return resultado;
        }

        private static void RegistrarEnArregloTexto(ref string[] arreglo, ref int contador, string valor)
        {
            if (contador >= arreglo.Length)
            {
                string[] nuevo = new string[arreglo.Length * 2];
                for (int i = 0; i < arreglo.Length; i++)
                {
                    nuevo[i] = arreglo[i];
                }
                arreglo = nuevo;
            }
            arreglo[contador] = valor;
            contador++;
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
    }
}