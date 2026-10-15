using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prueba_Proyecto_3
{
    public partial class RegistroAdultoMayorForm : Form
    {
        private IRepositorioPaciente repositorio = new RepositorioSql();

        // Controles generales
        private TextBox txtNombre, txtExpediente, txtEdad, txtDireccion, txtResponsable;
        private ComboBox cmbSexo;
        private TextBox txtConsultaPor, txtPresenteEnfermedad;

        // Examen Físico (ComboBox para Normal/Anormal y TextBox para detalle por región)
        private Dictionary<string, ComboBox> cmbsExamenFisicoEstado = new Dictionary<string, ComboBox>();
        private Dictionary<string, TextBox> txtsExamenFisicoDetalle = new Dictionary<string, TextBox>();

        // Signos Vitales
        private TextBox txtTemp, txtFR, txtFC, txtPulso, txtPA, txtSpO2, txtPeso, txtTalla, txtCirAbdominal;

        // ComboBoxes para el Índice de Barthel (10 ítems)
        private ComboBox cmbComer, cmbBaniarse, cmbVestirse, cmbAseo, cmbDeposicion;
        private ComboBox cmbMiccion, cmbRetrete, cmbTrasladarse, cmbDeambulacion, cmbEscalones;

        // Diagnóstico, Tipo de Consulta y tratamiento
        private ComboBox cmbTipoDiagnostico;
        private ComboBox cmbTipoConsulta;
        private TextBox txtDiagnostico, txtReceta, txtRecomendaciones;
        private Button bGuardar;

        private void RegistroAdultoMayorForm_Load(object sender, EventArgs e)
        {
            // Lógica inicial del formulario al cargar, si se requiere
        }

        public RegistroAdultoMayorForm()
        {
            InitializeComponent();
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "Registro Especializado - Paciente Adulto Mayor";
            this.Size = new Size(800, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = true;

            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.AutoScroll = true;
            this.Controls.Add(panel);

            int anchoContenidoUtil = 620;


            int offsetX = Math.Max(20, (this.ClientSize.Width - anchoContenidoUtil) / 2);

            int y = 20;


            Label lblTitulo = new Label();
            lblTitulo.Text = "EXPEDIENTE CLÍNICO: PACIENTE ADULTO MAYOR";
            lblTitulo.Font = new Font("Arial", 11f, FontStyle.Bold);
            lblTitulo.Location = new Point(offsetX, y);
            lblTitulo.Size = new Size(anchoContenidoUtil, 25);
            panel.Controls.Add(lblTitulo);
            y += 40;

            // --- 1. DATOS GENERALES ---
            AgregarSeccion(panel, "1. Datos Generales del Paciente", ref y);
            txtNombre = AgregarCampo(panel, "Nombre del Paciente:", ref y);
            txtExpediente = AgregarCampo(panel, "N° de Expediente:", ref y);
            txtEdad = AgregarCampo(panel, "Edad (60-120 años):", ref y);

            Label lblSx = new Label { Text = "Género:", Location = new Point(30, y), Size = new Size(180, 20), Font = new Font("Arial", 9, FontStyle.Bold) };
            cmbSexo = new ComboBox { Location = new Point(230, y), Size = new Size(300, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbSexo.Items.AddRange(new object[] { "Masculino", "Femenino" });
            cmbSexo.SelectedIndex = 0;
            panel.Controls.Add(lblSx);
            panel.Controls.Add(cmbSexo);
            y += 35;

            txtDireccion = AgregarCampo(panel, "Dirección:", ref y);
            txtResponsable = AgregarCampo(panel, "Cu cuidador / Responsable:", ref y);
            y += 10;

            // --- 2. MOTIVO DE CONSULTA ---
            AgregarSeccion(panel, "2. Motivo de Consulta", ref y);
            txtConsultaPor = AgregarCampo(panel, "Consulta Por:", ref y);
            txtPresenteEnfermedad = AgregarCampo(panel, "Presente Enfermedad:", ref y);
            y += 10;

            // --- 3. EXAMEN FÍSICO POR REGIONES (Antes de Signos Vitales) ---
            AgregarSeccion(panel, "3. Examen Físico por Regiones", ref y);
            string[] regiones = new string[]
            {
                "Apariencia general", "Cabeza", "Cuello",
                "Tórax y pulmonar", "Cardíaco", "Abdomen",
                "Columna Vertebral", "Región Glútea", "Genitales",
                "Extremidades", "Piel y anexos", "Neurologico"
            };

            foreach (var reg in regiones)
            {
                Label lblReg = new Label();
                lblReg.Text = reg;
                lblReg.Location = new Point(30, y);
                lblReg.Size = new Size(150, 20);
                lblReg.Font = new Font("Arial", 9, FontStyle.Regular);
                panel.Controls.Add(lblReg);

                ComboBox cmbEstado = new ComboBox();
                cmbEstado.Location = new Point(190, y);
                cmbEstado.Size = new Size(100, 23);
                cmbEstado.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbEstado.Items.AddRange(new object[] { "Normal", "Anormal" });
                cmbEstado.SelectedIndex = 0;
                panel.Controls.Add(cmbEstado);
                cmbsExamenFisicoEstado[reg] = cmbEstado;

                TextBox txtDetalle = new TextBox();
                txtDetalle.Text = "Sin hallazgos";
                txtDetalle.Location = new Point(305, y);
                txtDetalle.Size = new Size(330, 23);
                panel.Controls.Add(txtDetalle);
                txtsExamenFisicoDetalle[reg] = txtDetalle;

                y += 32;
            }
            y += 10;

            // --- 4. SIGNOS VITALES ---
            AgregarSeccion(panel, "4. Signos Vitales y Medidas Antropométricas", ref y);
            txtTemp = AgregarCampo(panel, "Temperatura (°C):", ref y);
            txtFR = AgregarCampo(panel, "Freq. Respiratoria (rpm):", ref y);
            txtFC = AgregarCampo(panel, "Freq. Cardíaca (lpm):", ref y);
            txtPulso = AgregarCampo(panel, "Pulso (ppm):", ref y);
            txtPA = AgregarCampo(panel, "Presión Arterial (mmHg):", ref y);
            txtSpO2 = AgregarCampo(panel, "Saturación Oxígeno (%):", ref y);
            txtPeso = AgregarCampo(panel, "Peso (kg):", ref y);
            txtTalla = AgregarCampo(panel, "Talla (cm):", ref y);
            txtCirAbdominal = AgregarCampo(panel, "Cir. Abdominal (cm):", ref y);
            y += 10;

            // --- 5. EVALUACIÓN ÍNDICE DE BARTHEL ---
            AgregarSeccion(panel, "5. Evaluación Geriátrica - Índice de Barthel (AVD)", ref y);

            cmbComer = AgregarBarthelCombo(panel, "1. Comer", new string[] { "0 - Incapaz", "5 - Necesita ayuda", "10 - Independiente" }, new int[] { 0, 5, 10 }, ref y);
            cmbBaniarse = AgregarBarthelCombo(panel, "2. Lavarse / Bañarse", new string[] { "0 - Dependiente", "5 - Independiente" }, new int[] { 0, 5 }, ref y);
            cmbVestirse = AgregarBarthelCombo(panel, "3. Vestirse", new string[] { "0 - Dependiente", "5 - Necesita ayuda", "10 - Independiente" }, new int[] { 0, 5, 10 }, ref y);
            cmbAseo = AgregarBarthelCombo(panel, "4. Aseo Personal", new string[] { "0 - Dependiente", "5 - Independiente" }, new int[] { 0, 5 }, ref y);
            cmbDeposicion = AgregarBarthelCombo(panel, "5. Control de Deposición", new string[] { "0 - Incontinente", "5 - Accidente ocasional", "10 - Continente" }, new int[] { 0, 5, 10 }, ref y);
            cmbMiccion = AgregarBarthelCombo(panel, "6. Control de Micción", new string[] { "0 - Incontinente", "5 - Accidente ocasional", "10 - Continente" }, new int[] { 0, 5, 10 }, ref y);
            cmbRetrete = AgregarBarthelCombo(panel, "7. Uso del Retrete", new string[] { "0 - Dependiente", "5 - Necesita ayuda", "10 - Independiente" }, new int[] { 0, 5, 10 }, ref y);
            cmbTrasladarse = AgregarBarthelCombo(panel, "8. Trasladarse (Sillón/Cama)", new string[] { "0 - Incapaz", "5 - Gran ayuda", "10 - Mínima ayuda", "15 - Independiente" }, new int[] { 0, 5, 10, 15 }, ref y);
            cmbDeambulacion = AgregarBarthelCombo(panel, "9. Deambulación (Caminar)", new string[] { "0 - Inmóvil", "5 - Silla de ruedas", "10 - Ayuda 1 persona", "15 - Independiente" }, new int[] { 0, 5, 10, 15 }, ref y);
            cmbEscalones = AgregarBarthelCombo(panel, "10. Escalones / Escaleras", new string[] { "0 - Incapaz", "5 - Necesita ayuda", "10 - Independiente" }, new int[] { 0, 5, 10 }, ref y);
            y += 10;

            // --- 6. DIAGNÓSTICO Y TRATAMIENTO ---
            AgregarSeccion(panel, "6. Diagnóstico y Prescripción", ref y);

            Label lblTipoDiag = new Label { Text = "Tipo de Diagnóstico:", Location = new Point(30, y), Size = new Size(190, 20), Font = new Font("Arial", 9, FontStyle.Bold) };
            cmbTipoDiagnostico = new ComboBox { Location = new Point(230, y), Size = new Size(300, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbTipoDiagnostico.Items.AddRange(new object[] { "Principal", "Secundario" });
            cmbTipoDiagnostico.SelectedIndex = 0;
            panel.Controls.Add(lblTipoDiag);
            panel.Controls.Add(cmbTipoDiagnostico);
            y += 35;

            Label lblTipoCons = new Label { Text = "Tipo de Consulta:", Location = new Point(30, y), Size = new Size(190, 20), Font = new Font("Arial", 9, FontStyle.Bold) };
            cmbTipoConsulta = new ComboBox { Location = new Point(230, y), Size = new Size(300, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbTipoConsulta.Items.AddRange(new object[] { "Primera vez", "Subsecuente" });
            cmbTipoConsulta.SelectedIndex = 0;
            panel.Controls.Add(lblTipoCons);
            panel.Controls.Add(cmbTipoConsulta);
            y += 35;

            txtDiagnostico = AgregarCampo(panel, "Diagnóstico Médico:", ref y);
            txtReceta = AgregarCampo(panel, "Medicamento y Dosis:", ref y);
            txtRecomendaciones = AgregarCampo(panel, "Recomendaciones:", ref y);
            y += 20;

            // Botón Guardar
            bGuardar = new Button();
            bGuardar.Text = "Guardar Expediente Adulto Mayor";
            bGuardar.Location = new Point(30, y);
            bGuardar.Size = new Size(520, 40);
            bGuardar.BackColor = Color.FromArgb(0, 122, 204);
            bGuardar.ForeColor = Color.White;
            bGuardar.Font = new Font("Arial", 10, FontStyle.Bold);
            bGuardar.Click += BGuardar_Click;
            panel.Controls.Add(bGuardar);
            y += 60;
        }

        private void AgregarSeccion(Panel panel, string titulo, ref int y)
        {
            Label lbl = new Label();
            lbl.Text = titulo;
            lbl.Font = new Font("Arial", 9.5f, FontStyle.Bold);
            lbl.ForeColor = Color.FromArgb(0, 102, 204);
            lbl.Location = new Point(20, y);
            lbl.Size = new Size(620, 20);
            panel.Controls.Add(lbl);
            y += 25;
        }

        private TextBox AgregarCampo(Panel panel, string etiqueta, ref int y)
        {
            Label lbl = new Label();
            lbl.Text = etiqueta;
            lbl.Location = new Point(30, y);
            lbl.Size = new Size(190, 20);
            panel.Controls.Add(lbl);

            TextBox txt = new TextBox();
            txt.Location = new Point(230, y);
            txt.Size = new Size(300, 23);
            panel.Controls.Add(txt);

            y += 32;
            return txt;
        }

        private ComboBox AgregarBarthelCombo(Panel panel, string pregunta, string[] opciones, int[] valores, ref int y)
        {
            Label lbl = new Label();
            lbl.Text = pregunta;
            lbl.Location = new Point(30, y);
            lbl.Size = new Size(250, 20);
            panel.Controls.Add(lbl);

            ComboBox cmb = new ComboBox();
            cmb.Location = new Point(290, y);
            cmb.Size = new Size(240, 25);
            cmb.DropDownStyle = ComboBoxStyle.DropDownList;

            for (int i = 0; i < opciones.Length; i++)
            {
                cmb.Items.Add(new OpcionItem(opciones[i], valores[i]));
            }
            cmb.SelectedIndex = 0;
            panel.Controls.Add(cmb);

            y += 32;
            return cmb;
        }

        private class OpcionItem
        {
            public string Texto { get; set; }
            public int Valor { get; set; }
            public OpcionItem(string texto, int valor) { Texto = texto; Valor = valor; }
            public override string ToString() { return Texto; }
        }

        private int ObtenerValorBarthel(ComboBox cmb)
        {
            if (cmb.SelectedItem is OpcionItem item) return item.Valor;
            return 0;
        }

        private void BGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtExpediente.Text))
                {
                    MessageBox.Show("El nombre y el número de expediente son obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtEdad.Text, out int edad) || edad < 60 || edad > 120)
                {
                    MessageBox.Show("Para el paciente adulto mayor, la edad debe estar entre 60 y 120 años.", "Error de Rango", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Instanciación de la subclase PacienteAdultoMayor
                PacienteAdultoMayor paciente = new PacienteAdultoMayor(
                    txtNombre.Text.Trim(),
                    edad,
                    txtExpediente.Text.Trim(),
                    cmbSexo.SelectedItem.ToString(),
                    string.IsNullOrWhiteSpace(txtDireccion.Text) ? "N/A" : txtDireccion.Text.Trim(),
                    string.IsNullOrWhiteSpace(txtResponsable.Text) ? "N/A" : txtResponsable.Text.Trim()
                );

                // Motivo
                paciente.ConsultaPor = txtConsultaPor.Text.Trim();
                paciente.PresenteEnfermedad = txtPresenteEnfermedad.Text.Trim();

                // Mapeo del Examen Físico completo
                string[] regiones = new string[]
                {
                    "Apariencia general", "Cabeza", "Cuello",
                    "Tórax y pulmonar", "Cardíaco", "Abdomen",
                    "Columna Vertebral", "Región Glútea", "Genitales",
                    "Extremidades", "Piel y anexos", "Neurologico"
                };

                foreach (var reg in regiones)
                {
                    string estadoSeleccionado = cmbsExamenFisicoEstado[reg].SelectedItem.ToString();
                    bool esNormal = (estadoSeleccionado == "Normal");
                    string detalle = txtsExamenFisicoDetalle[reg].Text.Trim();

                    if (string.IsNullOrWhiteSpace(detalle))
                    {
                        detalle = esNormal ? "Sin hallazgos / Normal" : "Alteración no especificada";
                    }

                    paciente.ExamenFisico[reg] = new HallazgoExamen
                    {
                        EsNormal = esNormal,
                        Detalle = detalle
                    };
                }

                // Signos vitales y medidas
                double.TryParse(txtTemp.Text, out double temp);
                int.TryParse(txtFR.Text, out int fr);
                int.TryParse(txtFC.Text, out int fc);
                int.TryParse(txtPulso.Text, out int pulso);
                double.TryParse(txtSpO2.Text, out double spo2);
                double.TryParse(txtPeso.Text, out double peso);
                double.TryParse(txtTalla.Text, out double talla);
                double.TryParse(txtCirAbdominal.Text, out double cir);

                paciente.TemperaturaCelsius = temp;
                paciente.FrecuenciaRespiratoria = fr;
                paciente.FrecuenciaCardiaca = fc;
                paciente.Pulso = pulso;
                paciente.PresionArterial = string.IsNullOrWhiteSpace(txtPA.Text) ? "130/85" : txtPA.Text.Trim();
                paciente.SaturacionOxigeno = spo2;
                paciente.PesoKg = peso;
                paciente.TallaCm = talla;
                paciente.CircunferenciaAbdominalCm = cir;

                // Cálculo automático del Índice de Barthel
                int sumaBarthel = ObtenerValorBarthel(cmbComer) +
                                  ObtenerValorBarthel(cmbBaniarse) +
                                  ObtenerValorBarthel(cmbVestirse) +
                                  ObtenerValorBarthel(cmbAseo) +
                                  ObtenerValorBarthel(cmbDeposicion) +
                                  ObtenerValorBarthel(cmbMiccion) +
                                  ObtenerValorBarthel(cmbRetrete) +
                                  ObtenerValorBarthel(cmbTrasladarse) +
                                  ObtenerValorBarthel(cmbDeambulacion) +
                                  ObtenerValorBarthel(cmbEscalones);

                paciente.PuntajeBarthel = sumaBarthel;

                if (sumaBarthel < 20) paciente.CategoriaDependencia = "Dependencia Total";
                else if (sumaBarthel <= 35) paciente.CategoriaDependencia = "Dependencia Grave";
                else if (sumaBarthel <= 55) paciente.CategoriaDependencia = "Dependencia Moderada";
                else if (sumaBarthel <= 90) paciente.CategoriaDependencia = "Dependencia Leve";
                else paciente.CategoriaDependencia = "Independiente";

                // Diagnóstico, tipo de consulta y tratamiento
                paciente.DiagnosticoTexto = txtDiagnostico.Text.Trim();
                paciente.TipoDiagnostico = cmbTipoDiagnostico.SelectedItem.ToString();
                paciente.TipoConsulta = cmbTipoConsulta.SelectedItem.ToString();
                paciente.Recomendaciones = txtRecomendaciones.Text.Trim();

                if (!string.IsNullOrWhiteSpace(txtReceta.Text))
                {
                    paciente.Receta.Add(new Medicamento("Medicamento Geriátrico", txtReceta.Text.Trim()));
                }

                // Guardar en base de datos mediante Entity Framework
                repositorio.RegistrarPaciente(paciente);

                MessageBox.Show($"¡Expediente de Adulto Mayor guardado con éxito!\nPuntaje Barthel: {sumaBarthel} pts ({paciente.CategoriaDependencia})", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al guardar el registro: {ex.Message}", "Error de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
