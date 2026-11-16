// Autores: Gael Zair Limón Rodríguez, Sebastián Vázquez Hernández y Ervin Axel Rocha Mireles
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using SistemaLogisticaAuditoria.Dominio;
using SistemaLogisticaAuditoria.Servicios.Logica;
using SistemaLogisticaAuditoria.Servicios.Persistencia;

namespace SistemaLogisticaAuditoria
{
    public partial class Form1 : Form
    {
        private readonly ServicioConfiguracionJson _servicioJson;
        private readonly ServicioExportacionCsv _servicioCsv;
        private readonly GestorIdioma _gestorIdioma;
        private Etiquetas _etiquetasSeleccionadas;
        private ConfiguracionTarifas _configuracion;
        private ServicioCalculoFinanciero _servicioFinanciero;
        private ServicioProcesamientoLotes _servicioLotes;
        private RepositorioBaseDatos _repositorioBd;
        private ResultadoProcesamientoLote? _resultadoActual;
        private bool _actualizandoComboIdioma;

        public Form1()
        {
            InitializeComponent();

            string rutaJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            _servicioJson = new ServicioConfiguracionJson(rutaJson);
            _servicioCsv = new ServicioExportacionCsv();
            _gestorIdioma = new GestorIdioma();
            _etiquetasSeleccionadas = _gestorIdioma.EtiquetasSeleccionadas;

            // RF1: Lectura obligatoria del archivo JSON al iniciar el programa
            _configuracion = _servicioJson.CargarConfiguracion();
            _servicioFinanciero = new ServicioCalculoFinanciero(_configuracion);
            _servicioLotes = new ServicioProcesamientoLotes(_servicioFinanciero);
            _repositorioBd = new RepositorioBaseDatos(_configuracion.RutaBaseDatos);

            ConfigurarColumnasTabla();
            CargarLoteEjemploEnCaja();

            // RF2: Carga las etiquetas desde Recursos\etiquetas.json (Español, English, Français, Deutsch, Português)
            CargarEtiquetas(0);

            if (!_servicioJson.ExistiaArchivoAlIniciar)
            {
                MessageBox.Show(
                    _etiquetasSeleccionadas.MsgJsonFaltante,
                    "Configuración JSON (RF1)",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        /**
         * El siguiente método permite cargar las etiquetas de la interfaz de usuario desde Recursos\etiquetas.json.
         * Recibe un índice de lenguaje y carga las etiquetas correspondientes en los controles de la interfaz.
         */
        public void CargarEtiquetas(int indiceLenguaje)
        {
            string archivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Recursos", "etiquetas.json");
            if (!File.Exists(archivo))
            {
                archivo = "Recursos\\etiquetas.json";
            }

            if (!File.Exists(archivo))
            {
                MessageBox.Show("No se encontró el archivo de etiquetas de idioma (Recursos\\etiquetas.json).");
                return;
            }

            try
            {
                string jsonString = File.ReadAllText(archivo, Encoding.UTF8);
                List<Etiquetas>? etiquetas = JsonSerializer.Deserialize<List<Etiquetas>>(jsonString);
                if (etiquetas == null || etiquetas.Count == 0)
                    return;

                _actualizandoComboIdioma = true;
                cmbIdioma.Items.Clear();
                foreach (var etiqueta in etiquetas)
                {
                    cmbIdioma.Items.Add(etiqueta.Idioma);
                }
                if (indiceLenguaje < 0 || indiceLenguaje >= etiquetas.Count)
                {
                    indiceLenguaje = 0;
                }
                cmbIdioma.SelectedIndex = indiceLenguaje;
                _actualizandoComboIdioma = false;

                _etiquetasSeleccionadas = _gestorIdioma.SeleccionarLenguaje(indiceLenguaje);

                this.Text = _etiquetasSeleccionadas.TituloVentana;
                lblIdioma.Text = _etiquetasSeleccionadas.LblIdioma;
                btnCargarEjemplo.Text = _etiquetasSeleccionadas.BtnCargarEjemplo;
                grpCaptura.Text = _etiquetasSeleccionadas.GrpCaptura;
                btnProcesar.Text = _etiquetasSeleccionadas.BtnProcesar;
                btnInvertir.Text = _etiquetasSeleccionadas.BtnInvertir;
                btnMarcarRecibido.Text = _etiquetasSeleccionadas.BtnMarcarRecibido;
                btnExportarCsv.Text = _etiquetasSeleccionadas.BtnExportarCsv;
                btnHistorialBd.Text = _etiquetasSeleccionadas.BtnHistorialBd;
                grpTabla.Text = _etiquetasSeleccionadas.GrpTabla;
                grpBitacora.Text = _etiquetasSeleccionadas.GrpBitacora;
                grpInspeccion.Text = _etiquetasSeleccionadas.GrpInspeccion;

                if (dgvPaquetes.Columns.Count >= 13)
                {
                    dgvPaquetes.Columns[0].HeaderText = _etiquetasSeleccionadas.ColCodigo;
                    dgvPaquetes.Columns[1].HeaderText = _etiquetasSeleccionadas.ColCliente;
                    dgvPaquetes.Columns[2].HeaderText = _etiquetasSeleccionadas.ColZona;
                    dgvPaquetes.Columns[3].HeaderText = _etiquetasSeleccionadas.ColPeso;
                    dgvPaquetes.Columns[4].HeaderText = _etiquetasSeleccionadas.ColValor;
                    dgvPaquetes.Columns[5].HeaderText = _etiquetasSeleccionadas.ColSubPeso;
                    dgvPaquetes.Columns[6].HeaderText = _etiquetasSeleccionadas.ColRecargo;
                    dgvPaquetes.Columns[7].HeaderText = _etiquetasSeleccionadas.ColPenal;
                    dgvPaquetes.Columns[8].HeaderText = _etiquetasSeleccionadas.ColSeguro;
                    dgvPaquetes.Columns[9].HeaderText = _etiquetasSeleccionadas.ColSubNeto;
                    dgvPaquetes.Columns[10].HeaderText = _etiquetasSeleccionadas.ColIVA;
                    dgvPaquetes.Columns[11].HeaderText = _etiquetasSeleccionadas.ColTotal;
                    dgvPaquetes.Columns[12].HeaderText = _etiquetasSeleccionadas.ColEstado;
                }

                ActualizarBannerConfiguracion();

                if (_resultadoActual != null)
                {
                    ActualizarResumenLote(_resultadoActual);
                    ProyectarPaqueteSeleccionado();
                }
            }
            catch (JsonException er)
            {
                _actualizandoComboIdioma = false;
                MessageBox.Show("Error al leer el archivo JSON de idiomas: " + er.Message);
            }
        }

        private void ConfigurarColumnasTabla()
        {
            dgvPaquetes.AutoGenerateColumns = false;
            dgvPaquetes.Columns.Clear();

            AgregarColumna("colCodigo", "ColCodigo", 58);
            AgregarColumna("colCliente", "ColCliente", 104);
            AgregarColumna("colZona", "ColZona", 52);
            AgregarColumna("colPeso", "ColPeso", 52);
            AgregarColumna("colValor", "ColValor", 62);
            AgregarColumna("colSubPeso", "ColSubPeso", 58);
            AgregarColumna("colRecargo", "ColRecargo", 54);
            AgregarColumna("colPenal", "ColPenal", 50);
            AgregarColumna("colSeguro", "ColSeguro", 48);
            AgregarColumna("colSubNeto", "ColSubNeto", 60);
            AgregarColumna("colIVA", "ColIVA", 48);
            AgregarColumna("colTotal", "ColTotal", 66);
            AgregarColumna("colEstado", "ColEstado", 52);
        }

        private void AgregarColumna(string nombre, string claveIdioma, int ancho)
        {
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn
            {
                Name = nombre,
                HeaderText = _gestorIdioma.ObtenerTexto(claveIdioma),
                Width = ancho,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
            dgvPaquetes.Columns.Add(col);
        }

        public void CargarLoteEjemploEnCaja()
        {
            string[] lineas = new[]
            {
                " COD-101 ; Cliente Juan Perez ; Zona A ; 15.5 ; 200.0 ",
                " PACK-001 ; Ana Sofia Martinez ; Zona B ; 24.0 ; 1500.0 ",
                "PACK-002; Maria Lopez; Zona C; -5.0; 200.0",
                "PACK-003; Pedro Gomez; Zona B; ABC; 100.0",
                " PACK-004 ; Distribuidora Del Centro ; Zona C ; 32.5 ; 3400.0 ",
                " PACK-005 ; Carlos Eduardo Ruiz ; Zona A ; 8.0 ; 500.0 ",
                " PACK-006 ; Importaciones Bajio ; Zona C ; 28.0 ; 2100.0 ",
                " PACK-007 ; Ferreteria Aguascalientes ; Zona B ; 18.0 ; 1250.0 ",
                " PACK-008 ; Farmacia San Marcos ; Zona A ; 22.0 ; 950.0 "
            };
            txtLoteEntrada.Text = string.Join(Environment.NewLine, lineas);
        }

        private void ActualizarBannerConfiguracion()
        {
            lblConfigJson.Text =
                $"[JSON RF1: appsettings.json] Base/kg: ${_configuracion.PrecioBasePorKg:N2} | {_etiquetasSeleccionadas.ColIVA}: {_configuracion.TasaIVA * 100:F0}% | Fijo: ${_configuracion.CostoFijoEnvio:N2} | >{_configuracion.PesoLimiteKg}kg (+25%) | >${_configuracion.MontoMinimoSeguro:N0} (+2%) | BD: {_configuracion.RutaBaseDatos}";
        }

        /**
         * El siguiente método permite cambiar el idioma de la interfaz de usuario (RF2).
         * Se ejecuta cuando se selecciona un nuevo idioma en el combo box cmbIdioma.
         */
        private void cmbIdioma_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_actualizandoComboIdioma)
                return;

            if (cmbIdioma.SelectedIndex >= 0)
            {
                CargarEtiquetas(cmbIdioma.SelectedIndex);
            }
        }

        private void btnCargarEjemplo_Click(object sender, EventArgs e)
        {
            CargarLoteEjemploEnCaja();
        }

        private void btnProcesar_Click(object sender, EventArgs e)
        {
            try
            {
                _resultadoActual = _servicioLotes.ProcesarTextoLote(txtLoteEntrada.Text, _gestorIdioma.IndiceLenguajeActual != 0);

                // Guardar las transacciones de envío en la Base de Datos (RF4)
                _repositorioBd.GuardarLoteEnBaseDatos(_resultadoActual.PaquetesValidos);

                PoblarTablaPaquetes(_resultadoActual.PaquetesValidos);
                PoblarBitacoraYErrores(_resultadoActual);
                ActualizarResumenLote(_resultadoActual);

                if (dgvPaquetes.Rows.Count > 0)
                {
                    dgvPaquetes.ClearSelection();
                    dgvPaquetes.Rows[0].Selected = true;
                    ProyectarPaquetePorIndice(0);
                }
                else
                {
                    txtInspeccion.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Diagnóstico de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PoblarTablaPaquetes(Paquete[] paquetes)
        {
            dgvPaquetes.Rows.Clear();
            for (int i = 0; i < paquetes.Length; i++)
            {
                Paquete p = paquetes[i];
                dgvPaquetes.Rows.Add(
                    p.Codigo,
                    p.Cliente,
                    p.Zona,
                    $"{p.PesoKg:F2}",
                    $"${p.ValorDeclarado:N2}",
                    $"${p.SubtotalPeso:N2}",
                    $"${p.RecargoZona:N2}",
                    $"${p.PenalizacionSobrepeso:N2}",
                    $"${p.SeguroValorDeclarado:N2}",
                    $"${p.SubtotalNeto:N2}",
                    $"${p.MontoIVA:N2}",
                    $"${p.TotalPagar:N2}",
                    p.EstadoTransaccion);
            }
        }

        private void PoblarBitacoraYErrores(ResultadoProcesamientoLote resultado)
        {
            lstBitacoraErrores.Items.Clear();

            for (int i = 0; i < resultado.BitacoraArreglos.Length; i++)
            {
                lstBitacoraErrores.Items.Add(resultado.BitacoraArreglos[i]);
            }

            if (resultado.ErroresDetectados.Length > 0)
            {
                lstBitacoraErrores.Items.Add(_gestorIdioma.IndiceLenguajeActual != 0
                    ? $"--- PARALLEL DISCARDED ERROR ARRAY ({resultado.ErroresDetectados.Length} corrupt lines) ---"
                    : $"--- ARREGLO PARALELO DE ERRORES DESCARTADOS ({resultado.ErroresDetectados.Length} líneas corruptas) ---");

                for (int i = 0; i < resultado.ErroresDetectados.Length; i++)
                {
                    lstBitacoraErrores.Items.Add("✖ " + resultado.ErroresDetectados[i]);
                }
            }
        }

        private void ActualizarResumenLote(ResultadoProcesamientoLote r)
        {
            txtTotalesLote.Text = _gestorIdioma.FormatearResumenLote(r);
        }

        // RF5: Evento de clic o cambio de fila seleccionada en DataGridView para proyectar el desglose en el panel lateral
        private void dgvPaquetes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ProyectarPaquetePorIndice(e.RowIndex);
            }
        }

        private void dgvPaquetes_SelectionChanged(object sender, EventArgs e)
        {
            ProyectarPaqueteSeleccionado();
        }

        private void ProyectarPaqueteSeleccionado()
        {
            if (dgvPaquetes.CurrentRow != null && dgvPaquetes.CurrentRow.Index >= 0)
            {
                ProyectarPaquetePorIndice(dgvPaquetes.CurrentRow.Index);
            }
        }

        private void ProyectarPaquetePorIndice(int indice)
        {
            if (_resultadoActual == null || indice < 0 || indice >= _resultadoActual.PaquetesValidos.Length)
                return;

            Paquete p = _resultadoActual.PaquetesValidos[indice];
            txtInspeccion.Text = _gestorIdioma.FormatearInspeccionPaquete(p, _configuracion);
        }

        // Reto 2: Botón para invertir manualmente el arreglo estático de paquetes
        private void btnInvertir_Click(object sender, EventArgs e)
        {
            if (_resultadoActual == null || _resultadoActual.PaquetesValidos.Length == 0)
            {
                MessageBox.Show(_etiquetasSeleccionadas.MsgSinPaquetes, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ServicioProcesamientoLotes.InvertirArregloPaquetes(_resultadoActual.PaquetesValidos);
            PoblarTablaPaquetes(_resultadoActual.PaquetesValidos);
            lstBitacoraErrores.Items.Insert(0, _gestorIdioma.IndiceLenguajeActual != 0
                ? "[Reverse] Package array manually inverted using index swap."
                : "[Inversión] Arreglo de paquetes invertido manualmente intercambiando extremos.");
            ProyectarPaquetePorIndice(0);
        }

        // RF4 (Persistencia en Base de Datos de Recepciones)
        private void btnMarcarRecibido_Click(object sender, EventArgs e)
        {
            if (_resultadoActual == null || dgvPaquetes.CurrentRow == null)
            {
                MessageBox.Show(_etiquetasSeleccionadas.MsgSeleccioneFila, "Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int idx = dgvPaquetes.CurrentRow.Index;
            if (idx < 0 || idx >= _resultadoActual.PaquetesValidos.Length)
                return;

            Paquete p = _resultadoActual.PaquetesValidos[idx];
            string nuevoEstado = (_etiquetasSeleccionadas.EstadosTransaccion != null && _etiquetasSeleccionadas.EstadosTransaccion.Count >= 3)
                ? _etiquetasSeleccionadas.EstadosTransaccion[2]
                : "RECIBIDO EN DESTINO";
            p.EstadoTransaccion = nuevoEstado;
            _repositorioBd.ActualizarEstadoRecepcion(p.Codigo, p.EstadoTransaccion);

            dgvPaquetes.Rows[idx].Cells["colEstado"].Value = p.EstadoTransaccion;
            ProyectarPaquetePorIndice(idx);

            lstBitacoraErrores.Items.Insert(0, $"[BD SQL] UPDATE EstadoTransaccion = '{nuevoEstado}' WHERE CodigoPaquete = '{p.Codigo}'");
        }

        // RF6: Exportación de reportes a CSV mediante SaveFileDialog
        private void btnExportarCsv_Click(object sender, EventArgs e)
        {
            if (_resultadoActual == null || _resultadoActual.PaquetesValidos.Length == 0)
            {
                MessageBox.Show(_etiquetasSeleccionadas.MsgSinPaquetes, "Exportar CSV", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog dialogoGuardar = new SaveFileDialog())
            {
                dialogoGuardar.Filter = "Archivos CSV (*.csv)|*.csv|Todos los archivos (*.*)|*.*";
                dialogoGuardar.Title = _etiquetasSeleccionadas.BtnExportarCsv;
                dialogoGuardar.FileName = $"Auditoria_Fletes_{DateTime.Now:yyyyMMdd_HHmm}.csv";

                if (dialogoGuardar.ShowDialog(this) == DialogResult.OK)
                {
                    ExportarCsvDirecto(dialogoGuardar.FileName);
                    MessageBox.Show(
                        _etiquetasSeleccionadas.MsgCsvExito + dialogoGuardar.FileName,
                        "CSV (RF6)",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }

        public void ExportarCsvDirecto(string rutaCsv)
        {
            if (_resultadoActual != null)
            {
                _servicioCsv.ExportarResultadoACsv(_resultadoActual, rutaCsv);
            }
        }

        private void btnHistorialBd_Click(object sender, EventArgs e)
        {
            string[] registros = _repositorioBd.ConsultarUltimasTransacciones(30);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"=== SQLITE DB ({_configuracion.RutaBaseDatos}) ===");
            sb.AppendLine($"Total: {registros.Length}");
            sb.AppendLine("--------------------------------------------");

            for (int i = 0; i < registros.Length; i++)
            {
                sb.AppendLine(registros[i]);
            }

            txtInspeccion.Text = sb.ToString();
        }
    }
}