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


    public partial class RegistroAdolescenteForm : Form
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

        // Antecedentes Familiares (9 ítems)
        private Dictionary<string, CheckBox> chksAntFam = new Dictionary<string, CheckBox>();

        // Antecedentes Personales (10 ítems con sus paneles/controles para Año y Medicamento)
        private Dictionary<string, CheckBox> chksAntPers = new Dictionary<string, CheckBox>();
        private Dictionary<string, TextBox> txtsAntPersAnio = new Dictionary<string, TextBox>();
        private Dictionary<string, TextBox> txtsAntPersMed = new Dictionary<string, TextBox>();

        // Alcohol, Drogas y Evaluaciones
        private CheckBox chkEtilista, chkTabaquista, chkDrogadiccion;
        private TextBox txtEscalaTanner, txtPanelSnellen;

        // Laboratorios Adolescente (5 ítems)
        private CheckBox chkLabHemograma, chkLabColesterol, chkTrigliceridos, chkLabHeces, chkLabOrina;

        // Diagnóstico, Tipo de Consulta y Tratamiento
        private ComboBox cmbTipoDiagnostico;
        private ComboBox cmbTipoConsulta;
        private TextBox txtDiagnostico, txtReceta, txtRecomendaciones;
        private Button bGuardar;

        private void RegistroAdolescenteForm_Load(object sender, EventArgs e)
        {
            // Lógica inicial del formulario al cargar, si se requiere
        }

        public RegistroAdolescenteForm()
        {
            InitializeComponent();
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "Registro Especializado - Paciente Adolescente";
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
            lblTitulo.Text = "EXPEDIENTE CLÍNICO: PACIENTE ADOLESCENTE";
            lblTitulo.Font = new Font("Arial", 11f, FontStyle.Bold);
            lblTitulo.Location = new Point(offsetX, y);
            lblTitulo.Size = new Size(anchoContenidoUtil, 25);
            panel.Controls.Add(lblTitulo);
            y += 40;

            // --- 1. DATOS GENERALES ---
            AgregarSeccion(panel, "1. Datos Generales del Paciente", ref y);
            txtNombre = AgregarCampo(panel, "Nombre del Paciente:", ref y);
            txtExpediente = AgregarCampo(panel, "N° de Expediente:", ref y);
            txtEdad = AgregarCampo(panel, "Edad (12-17 años):", ref y);

            Label lblSx = new Label { Text = "Género:", Location = new Point(30, y), Size = new Size(210, 20), Font = new Font("Arial", 9, FontStyle.Bold) };
            cmbSexo = new ComboBox { Location = new Point(250, y), Size = new Size(300, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbSexo.Items.AddRange(new object[] { "Masculino", "Femenino" });
            cmbSexo.SelectedIndex = 0;
            panel.Controls.Add(lblSx);
            panel.Controls.Add(cmbSexo);
            y += 35;

            txtDireccion = AgregarCampo(panel, "Dirección:", ref y);
            txtResponsable = AgregarCampo(panel, "Responsable:", ref y);
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
                txtDetalle.Size = new Size(350, 23);
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

            // --- 5. ANTECEDENTES FAMILIARES PATOLÓGICOS ---
            AgregarSeccion(panel, "5. Antecedentes Familiares Patológicos", ref y);
            string[] antFam = new string[]
            {
                "Hipertensión arterial", "Trastornos mentales y de comportamiento",
                "Enfermedad renal crónica", "Infecciones", "Obesidad",
                "Otras enfermedades cardiovasculares", "Diabetes mellitus",
                "Asma bronquial", "Cáncer"
            };

            foreach (var item in antFam)
            {
                CheckBox chk = AgregarCheckbox(panel, $"Antecedente familiar: {item}", ref y);
                chksAntFam[item] = chk;
            }
            y += 10;

            // --- 6. ANTECEDENTES PERSONALES PATOLÓGICOS ---
            AgregarSeccion(panel, "6. Antecedentes Personales Patológicos", ref y);
            string[] antPers = new string[]
            {
                "Hipertensión arterial", "Otras enfermedades cardiovasculares",
                "Epilepsia", "EPOC", "Enfermedades infectocontagiosas",
                "Cáncer", "Asma bronquial", "Trastornos mentales",
                "Artritis", "Obesidad"
            };

            foreach (var item in antPers)
            {
                CheckBox chk = AgregarCheckbox(panel, $"¿Padece {item}?", ref y);
                chksAntPers[item] = chk;

                TextBox txtAnio = AgregarCampo(panel, $"   -> Año de inicio de {item}:", ref y);
                txtAnio.Text = DateTime.Now.Year.ToString();
                txtsAntPersAnio[item] = txtAnio;

                TextBox txtMed = AgregarCampo(panel, $"   -> Medicamento recetado:", ref y);
                txtMed.Text = "Ninguno";
                txtsAntPersMed[item] = txtMed;
            }
            y += 10;

            // --- 7. USO DE ALCOHOL Y DROGAS ---
            AgregarSeccion(panel, "7. Estilos de Vida / Consumo", ref y);
            chkEtilista = AgregarCheckbox(panel, "Etilista", ref y);
            chkTabaquista = AgregarCheckbox(panel, "Tabaquista", ref y);
            chkDrogadiccion = AgregarCheckbox(panel, "Drogadicción", ref y);
            y += 10;

            // --- 8. EVALUACIONES ADICIONALES ---
            AgregarSeccion(panel, "8. Evaluaciones Clínicas Adicionales", ref y);
            txtEscalaTanner = AgregarCampo(panel, "Escala de Tanner:", ref y);
            txtPanelSnellen = AgregarCampo(panel, "Panel de Snellen (Agudeza Visual):", ref y);
            y += 10;

            // --- 9. SOLICITUD DE ESTUDIOS DE LABORATORIO ---
            AgregarSeccion(panel, "9. Solicitud de Estudios de Laboratorio", ref y);
            chkLabHemograma = AgregarCheckbox(panel, "Hemograma automatizado (Sangre)", ref y);
            chkLabColesterol = AgregarCheckbox(panel, "Colesterol (Sangre)", ref y);
            chkTrigliceridos = AgregarCheckbox(panel, "Triglicéridos (Sangre)", ref y);
            chkLabHeces = AgregarCheckbox(panel, "General de Heces (Heces)", ref y);
            chkLabOrina = AgregarCheckbox(panel, "General de Orina (Orina)", ref y);
            y += 10;

            // --- 10. DIAGNÓSTICO Y TRATAMIENTO ---
            AgregarSeccion(panel, "10. Diagnóstico y Prescripción", ref y);

            Label lblTipoDiag = new Label { Text = "Tipo de Diagnóstico:", Location = new Point(30, y), Size = new Size(210, 20), Font = new Font("Arial", 9, FontStyle.Bold) };
            cmbTipoDiagnostico = new ComboBox { Location = new Point(250, y), Size = new Size(300, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbTipoDiagnostico.Items.AddRange(new object[] { "Principal", "Secundario" });
            cmbTipoDiagnostico.SelectedIndex = 0;
            panel.Controls.Add(lblTipoDiag);
            panel.Controls.Add(cmbTipoDiagnostico);
            y += 35;

            Label lblTipoCons = new Label { Text = "Tipo de Consulta:", Location = new Point(30, y), Size = new Size(210, 20), Font = new Font("Arial", 9, FontStyle.Bold) };
            cmbTipoConsulta = new ComboBox { Location = new Point(250, y), Size = new Size(300, 25), DropDownStyle = ComboBoxStyle.DropDownList };
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
            bGuardar.Text = "Guardar Expediente Adolescente";
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
            lbl.Size = new Size(640, 20);
            panel.Controls.Add(lbl);
            y += 25;
        }

        private TextBox AgregarCampo(Panel panel, string etiqueta, ref int y)
        {
            Label lbl = new Label();
            lbl.Text = etiqueta;
            lbl.Location = new Point(30, y);
            lbl.Size = new Size(210, 20);
            panel.Controls.Add(lbl);

            TextBox txt = new TextBox();
            txt.Location = new Point(250, y);
            txt.Size = new Size(300, 23);
            panel.Controls.Add(txt);

            y += 32;
            return txt;
        }

        private CheckBox AgregarCheckbox(Panel panel, string texto, ref int y)
        {
            CheckBox chk = new CheckBox();
            chk.Text = texto;
            chk.Location = new Point(30, y);
            chk.Size = new Size(500, 24);
            panel.Controls.Add(chk);
            y += 28;
            return chk;
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

                if (!int.TryParse(txtEdad.Text, out int edad) || edad < 12 || edad > 17)
                {
                    MessageBox.Show("Para paciente adolescente, la edad debe estar entre 12 y 17 años.", "Error de Rango", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Instanciación de PacienteAdolescente
                PacienteAdolescente paciente = new PacienteAdolescente(
                    txtNombre.Text.Trim(),
                    edad,
                    txtExpediente.Text.Trim(),
                    cmbSexo.SelectedItem.ToString(),
                    string.IsNullOrWhiteSpace(txtDireccion.Text) ? "N/A" : txtDireccion.Text.Trim(),
                    string.IsNullOrWhiteSpace(txtResponsable.Text) ? "N/A" : txtResponsable.Text.Trim()
                );

                // Motivo y enfermedad actual
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
                paciente.PresionArterial = string.IsNullOrWhiteSpace(txtPA.Text) ? "110/70" : txtPA.Text.Trim();
                paciente.SaturacionOxigeno = spo2;
                paciente.PesoKg = peso;
                paciente.TallaCm = talla;
                paciente.CircunferenciaAbdominalCm = cir;

                // Antecedentes Familiares Patológicos
                foreach (var kvp in chksAntFam)
                {
                    paciente.AntecedentesFamiliares[kvp.Key] = kvp.Value.Checked;
                }

                // Antecedentes Personales Patológicos
                foreach (var kvp in chksAntPers)
                {
                    if (kvp.Value.Checked)
                    {
                        int.TryParse(txtsAntPersAnio[kvp.Key].Text, out int anioInicio);
                        string medicamento = txtsAntPersMed[kvp.Key].Text.Trim();

                        paciente.AntecedentesPersonales.Add(new AntecedentePersonal
                        {
                            Enfermedad = kvp.Key,
                            AnioInicio = anioInicio,
                            TipoMedication = string.IsNullOrWhiteSpace(medicamento) ? "N/A" : medicamento
                        });
                    }
                }

                // Estilos de vida y consumo
                paciente.Etilista = chkEtilista.Checked;
                paciente.Tabaquista = chkTabaquista.Checked;
                paciente.Drogadiccion = chkDrogadiccion.Checked;

                // Evaluaciones adicionales
                paciente.EscalaTanner = string.IsNullOrWhiteSpace(txtEscalaTanner.Text) ? "N/A" : txtEscalaTanner.Text.Trim();
                paciente.PanelSnellen = string.IsNullOrWhiteSpace(txtPanelSnellen.Text) ? "N/A" : txtPanelSnellen.Text.Trim();

                // Solicitud de Laboratorios
                if (chkLabHemograma.Checked) paciente.LaboratoriosSolicitados.Add("Hemograma automatizado (Muestra: Sangre)");
                if (chkLabColesterol.Checked) paciente.LaboratoriosSolicitados.Add("Colesterol (Muestra: Sangre)");
                if (chkTrigliceridos.Checked) paciente.LaboratoriosSolicitados.Add("Triglicéridos (Muestra: Sangre)");
                if (chkLabHeces.Checked) paciente.LaboratoriosSolicitados.Add("General de Heces (Muestra: Heces)");
                if (chkLabOrina.Checked) paciente.LaboratoriosSolicitados.Add("General de Orina (Muestra: Orina)");

                // Diagnóstico, tipo de consulta y tratamiento
                paciente.DiagnosticoTexto = txtDiagnostico.Text.Trim();
                paciente.TipoDiagnostico = cmbTipoDiagnostico.SelectedItem.ToString();
                paciente.TipoConsulta = cmbTipoConsulta.SelectedItem.ToString();
                paciente.Recomendaciones = txtRecomendaciones.Text.Trim();

                if (!string.IsNullOrWhiteSpace(txtReceta.Text))
                {
                    paciente.Receta.Add(new Medicamento("Medicamento Adolescente", txtReceta.Text.Trim()));
                }

                // Guardar mediante Entity Framework
                repositorio.RegistrarPaciente(paciente);

                MessageBox.Show("¡Expediente de Paciente Adolescente guardado con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al guardar el registro: {ex.Message}", "Error de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
