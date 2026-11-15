// Autor: Gael Zair Limon Rdz
namespace SistemaLogisticaAuditoria
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblConfigJson;
        private System.Windows.Forms.Label lblIdioma;
        private System.Windows.Forms.ComboBox cmbIdioma;
        private System.Windows.Forms.Button btnCargarEjemplo;

        private System.Windows.Forms.GroupBox grpCaptura;
        private System.Windows.Forms.TextBox txtLoteEntrada;
        private System.Windows.Forms.Button btnProcesar;
        private System.Windows.Forms.Button btnInvertir;
        private System.Windows.Forms.Button btnMarcarRecibido;
        private System.Windows.Forms.Button btnExportarCsv;
        private System.Windows.Forms.Button btnHistorialBd;

        private System.Windows.Forms.GroupBox grpTabla;
        private System.Windows.Forms.DataGridView dgvPaquetes;

        private System.Windows.Forms.GroupBox grpBitacora;
        private System.Windows.Forms.ListBox lstBitacoraErrores;

        private System.Windows.Forms.GroupBox grpInspeccion;
        private System.Windows.Forms.TextBox txtInspeccion;
        private System.Windows.Forms.TextBox txtTotalesLote;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblConfigJson = new System.Windows.Forms.Label();
            this.lblIdioma = new System.Windows.Forms.Label();
            this.cmbIdioma = new System.Windows.Forms.ComboBox();
            this.btnCargarEjemplo = new System.Windows.Forms.Button();
            this.grpCaptura = new System.Windows.Forms.GroupBox();
            this.txtLoteEntrada = new System.Windows.Forms.TextBox();
            this.btnProcesar = new System.Windows.Forms.Button();
            this.btnInvertir = new System.Windows.Forms.Button();
            this.btnMarcarRecibido = new System.Windows.Forms.Button();
            this.btnExportarCsv = new System.Windows.Forms.Button();
            this.btnHistorialBd = new System.Windows.Forms.Button();
            this.grpTabla = new System.Windows.Forms.GroupBox();
            this.dgvPaquetes = new System.Windows.Forms.DataGridView();
            this.grpBitacora = new System.Windows.Forms.GroupBox();
            this.lstBitacoraErrores = new System.Windows.Forms.ListBox();
            this.grpInspeccion = new System.Windows.Forms.GroupBox();
            this.txtInspeccion = new System.Windows.Forms.TextBox();
            this.txtTotalesLote = new System.Windows.Forms.TextBox();
            this.pnlHeader.SuspendLayout();
            this.grpCaptura.SuspendLayout();
            this.grpTabla.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPaquetes)).BeginInit();
            this.grpBitacora.SuspendLayout();
            this.grpInspeccion.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(238, 242, 255);
            this.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHeader.Controls.Add(this.lblConfigJson);
            this.pnlHeader.Controls.Add(this.lblIdioma);
            this.pnlHeader.Controls.Add(this.cmbIdioma);
            this.pnlHeader.Controls.Add(this.btnCargarEjemplo);
            this.pnlHeader.Location = new System.Drawing.Point(10, 8);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1160, 40);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblConfigJson
            // 
            this.lblConfigJson.AutoSize = true;
            this.lblConfigJson.Font = new System.Drawing.Font("Segoe UI Semibold", 8.8F, System.Drawing.FontStyle.Bold);
            this.lblConfigJson.ForeColor = System.Drawing.Color.FromArgb(30, 58, 138);
            this.lblConfigJson.Location = new System.Drawing.Point(10, 10);
            this.lblConfigJson.Name = "lblConfigJson";
            this.lblConfigJson.Size = new System.Drawing.Size(340, 15);
            this.lblConfigJson.TabIndex = 0;
            this.lblConfigJson.Text = "[Config JSON RF1: appsettings.json]";
            // 
            // lblIdioma
            // 
            this.lblIdioma.AutoSize = true;
            this.lblIdioma.Location = new System.Drawing.Point(772, 11);
            this.lblIdioma.Name = "lblIdioma";
            this.lblIdioma.Size = new System.Drawing.Size(105, 15);
            this.lblIdioma.TabIndex = 1;
            this.lblIdioma.Text = "Idioma / Language:";
            // 
            // cmbIdioma
            // 
            this.cmbIdioma.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbIdioma.Items.AddRange(new object[] {
            "Español (ES)",
            "English (EN)"});
            this.cmbIdioma.Location = new System.Drawing.Point(885, 7);
            this.cmbIdioma.Name = "cmbIdioma";
            this.cmbIdioma.Size = new System.Drawing.Size(112, 23);
            this.cmbIdioma.TabIndex = 2;
            this.cmbIdioma.SelectedIndexChanged += new System.EventHandler(this.cmbIdioma_SelectedIndexChanged);
            // 
            // btnCargarEjemplo
            // 
            this.btnCargarEjemplo.Location = new System.Drawing.Point(1004, 6);
            this.btnCargarEjemplo.Name = "btnCargarEjemplo";
            this.btnCargarEjemplo.Size = new System.Drawing.Size(148, 26);
            this.btnCargarEjemplo.TabIndex = 3;
            this.btnCargarEjemplo.Text = "Cargar Lote de Prueba";
            this.btnCargarEjemplo.UseVisualStyleBackColor = true;
            this.btnCargarEjemplo.Click += new System.EventHandler(this.btnCargarEjemplo_Click);
            // 
            // grpCaptura
            // 
            this.grpCaptura.Controls.Add(this.txtLoteEntrada);
            this.grpCaptura.Controls.Add(this.btnProcesar);
            this.grpCaptura.Controls.Add(this.btnInvertir);
            this.grpCaptura.Controls.Add(this.btnMarcarRecibido);
            this.grpCaptura.Controls.Add(this.btnExportarCsv);
            this.grpCaptura.Controls.Add(this.btnHistorialBd);
            this.grpCaptura.Location = new System.Drawing.Point(10, 54);
            this.grpCaptura.Name = "grpCaptura";
            this.grpCaptura.Size = new System.Drawing.Size(790, 172);
            this.grpCaptura.TabIndex = 1;
            this.grpCaptura.TabStop = false;
            this.grpCaptura.Text = "1. Captura de Lote de Paquetes (Formato: CODIGO;Cliente;Zona;PesoKg;ValorDeclarado)";
            // 
            // txtLoteEntrada
            // 
            this.txtLoteEntrada.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtLoteEntrada.Location = new System.Drawing.Point(12, 20);
            this.txtLoteEntrada.Multiline = true;
            this.txtLoteEntrada.Name = "txtLoteEntrada";
            this.txtLoteEntrada.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLoteEntrada.Size = new System.Drawing.Size(580, 140);
            this.txtLoteEntrada.TabIndex = 0;
            // 
            // btnProcesar
            // 
            this.btnProcesar.Font = new System.Drawing.Font("Segoe UI Semibold", 8.8F, System.Drawing.FontStyle.Bold);
            this.btnProcesar.Location = new System.Drawing.Point(602, 19);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(176, 28);
            this.btnProcesar.TabIndex = 1;
            this.btnProcesar.Text = "Procesar Lote y Guardar en BD";
            this.btnProcesar.UseVisualStyleBackColor = true;
            this.btnProcesar.Click += new System.EventHandler(this.btnProcesar_Click);
            // 
            // btnInvertir
            // 
            this.btnInvertir.Location = new System.Drawing.Point(602, 49);
            this.btnInvertir.Name = "btnInvertir";
            this.btnInvertir.Size = new System.Drawing.Size(176, 26);
            this.btnInvertir.TabIndex = 2;
            this.btnInvertir.Text = "Invertir Arreglo (Reto 2)";
            this.btnInvertir.UseVisualStyleBackColor = true;
            this.btnInvertir.Click += new System.EventHandler(this.btnInvertir_Click);
            // 
            // btnMarcarRecibido
            // 
            this.btnMarcarRecibido.Location = new System.Drawing.Point(602, 77);
            this.btnMarcarRecibido.Name = "btnMarcarRecibido";
            this.btnMarcarRecibido.Size = new System.Drawing.Size(176, 26);
            this.btnMarcarRecibido.TabIndex = 3;
            this.btnMarcarRecibido.Text = "Registrar Recepción (BD)";
            this.btnMarcarRecibido.UseVisualStyleBackColor = true;
            this.btnMarcarRecibido.Click += new System.EventHandler(this.btnMarcarRecibido_Click);
            // 
            // btnExportarCsv
            // 
            this.btnExportarCsv.Location = new System.Drawing.Point(602, 105);
            this.btnExportarCsv.Name = "btnExportarCsv";
            this.btnExportarCsv.Size = new System.Drawing.Size(176, 26);
            this.btnExportarCsv.TabIndex = 4;
            this.btnExportarCsv.Text = "Exportar a CSV (RF6)";
            this.btnExportarCsv.UseVisualStyleBackColor = true;
            this.btnExportarCsv.Click += new System.EventHandler(this.btnExportarCsv_Click);
            // 
            // btnHistorialBd
            // 
            this.btnHistorialBd.Location = new System.Drawing.Point(602, 133);
            this.btnHistorialBd.Name = "btnHistorialBd";
            this.btnHistorialBd.Size = new System.Drawing.Size(176, 26);
            this.btnHistorialBd.TabIndex = 5;
            this.btnHistorialBd.Text = "Ver Transacciones BD";
            this.btnHistorialBd.UseVisualStyleBackColor = true;
            this.btnHistorialBd.Click += new System.EventHandler(this.btnHistorialBd_Click);
            // 
            // grpTabla
            // 
            this.grpTabla.Controls.Add(this.dgvPaquetes);
            this.grpTabla.Location = new System.Drawing.Point(10, 232);
            this.grpTabla.Name = "grpTabla";
            this.grpTabla.Size = new System.Drawing.Size(790, 245);
            this.grpTabla.TabIndex = 2;
            this.grpTabla.TabStop = false;
            this.grpTabla.Text = "2. Paquetes Validados (Ordenados de Mayor a Menor Costo Total - Algoritmo Burbuja)";
            // 
            // dgvPaquetes
            // 
            this.dgvPaquetes.AllowUserToAddRows = false;
            this.dgvPaquetes.AllowUserToDeleteRows = false;
            this.dgvPaquetes.BackgroundColor = System.Drawing.Color.White;
            this.dgvPaquetes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPaquetes.Location = new System.Drawing.Point(12, 22);
            this.dgvPaquetes.MultiSelect = false;
            this.dgvPaquetes.Name = "dgvPaquetes";
            this.dgvPaquetes.ReadOnly = true;
            this.dgvPaquetes.RowHeadersVisible = false;
            this.dgvPaquetes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPaquetes.Size = new System.Drawing.Size(766, 210);
            this.dgvPaquetes.TabIndex = 0;
            this.dgvPaquetes.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPaquetes_CellClick);
            this.dgvPaquetes.SelectionChanged += new System.EventHandler(this.dgvPaquetes_SelectionChanged);
            // 
            // grpBitacora
            // 
            this.grpBitacora.Controls.Add(this.lstBitacoraErrores);
            this.grpBitacora.Location = new System.Drawing.Point(10, 483);
            this.grpBitacora.Name = "grpBitacora";
            this.grpBitacora.Size = new System.Drawing.Size(790, 145);
            this.grpBitacora.TabIndex = 3;
            this.grpBitacora.TabStop = false;
            this.grpBitacora.Text = "3. Arreglo Paralelo de Errores Descartados (Reto 1) y Bitácora de Redimensionamiento (Reto 2)";
            // 
            // lstBitacoraErrores
            // 
            this.lstBitacoraErrores.Font = new System.Drawing.Font("Consolas", 8.8F);
            this.lstBitacoraErrores.HorizontalScrollbar = true;
            this.lstBitacoraErrores.ItemHeight = 14;
            this.lstBitacoraErrores.Location = new System.Drawing.Point(12, 22);
            this.lstBitacoraErrores.Name = "lstBitacoraErrores";
            this.lstBitacoraErrores.Size = new System.Drawing.Size(766, 116);
            this.lstBitacoraErrores.TabIndex = 0;
            // 
            // grpInspeccion
            // 
            this.grpInspeccion.Controls.Add(this.txtInspeccion);
            this.grpInspeccion.Controls.Add(this.txtTotalesLote);
            this.grpInspeccion.Location = new System.Drawing.Point(808, 54);
            this.grpInspeccion.Name = "grpInspeccion";
            this.grpInspeccion.Size = new System.Drawing.Size(362, 574);
            this.grpInspeccion.TabIndex = 4;
            this.grpInspeccion.TabStop = false;
            this.grpInspeccion.Text = "4. Panel Lateral de Inspección y Totales del Lote (RF5)";
            // 
            // txtInspeccion
            // 
            this.txtInspeccion.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.txtInspeccion.Font = new System.Drawing.Font("Consolas", 8.8F);
            this.txtInspeccion.Location = new System.Drawing.Point(12, 22);
            this.txtInspeccion.Multiline = true;
            this.txtInspeccion.Name = "txtInspeccion";
            this.txtInspeccion.ReadOnly = true;
            this.txtInspeccion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInspeccion.Size = new System.Drawing.Size(338, 385);
            this.txtInspeccion.TabIndex = 0;
            // 
            // txtTotalesLote
            // 
            this.txtTotalesLote.BackColor = System.Drawing.Color.FromArgb(238, 242, 255);
            this.txtTotalesLote.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.txtTotalesLote.ForeColor = System.Drawing.Color.FromArgb(30, 58, 138);
            this.txtTotalesLote.Location = new System.Drawing.Point(12, 415);
            this.txtTotalesLote.Multiline = true;
            this.txtTotalesLote.Name = "txtTotalesLote";
            this.txtTotalesLote.ReadOnly = true;
            this.txtTotalesLote.Size = new System.Drawing.Size(338, 147);
            this.txtTotalesLote.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1182, 638);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.grpCaptura);
            this.Controls.Add(this.grpTabla);
            this.Controls.Add(this.grpBitacora);
            this.Controls.Add(this.grpInspeccion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sistema de Procesamiento y Auditoría de Facturación (Logística y Entregas)";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.grpCaptura.ResumeLayout(false);
            this.grpCaptura.PerformLayout();
            this.grpTabla.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPaquetes)).EndInit();
            this.grpBitacora.ResumeLayout(false);
            this.grpInspeccion.ResumeLayout(false);
            this.grpInspeccion.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion
    }
}
