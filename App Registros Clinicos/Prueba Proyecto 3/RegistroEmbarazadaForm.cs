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
    public partial class RegistroEmbarazadaForm : Form
    {
        private IRepositorioPaciente repositorio = new RepositorioSql();

        // Controles generales
        private TextBox txtNombre, txtExpediente, txtEdad, txtDireccion, txtResponsable;
        private TextBox txtConsultaPor, txtPresenteEnfermedad;

        // Examen Físico (ComboBox para Normal/Anormal y TextBox para detalle por región)
        private Dictionary<string, ComboBox> cmbsExamenFisicoEstado = new Dictionary<string, ComboBox>();
        private Dictionary<string, TextBox> txtsExamenFisicoDetalle = new Dictionary<string, TextBox>();

        private TextBox txtTemp, txtFR, txtFC, txtPulso, txtPA, txtSpO2, txtPeso, txtTalla, txtCirAbdominal;

        // Obstétricos
        private TextBox txtFormulaObstetrica, txtFinEmbarazoAntFecha, txtFinEmbarazoAntTerm, txtPesoAnterior, txtEdadGestacional;
        private CheckBox chkEmbarazoNoDeseado, chkUsabaAnticonceptivo;
        private TextBox txtMetodoAnticonceptivoEmp;
        private DateTimePicker dtpFUR;

        // Trimestres (T1, T2, T3 - Rellenados con Consola)
        private CheckBox chkT1FumaAct, chkT1FumaPas, chkT1Drogas, chkT1Alcohol, chkT1Violencia;
        private CheckBox chkT2FumaAct, chkT2FumaPas, chkT2Drogas, chkT2Alcohol, chkT2Violencia;
        private CheckBox chkT3FumaAct, chkT3FumaPas, chkT3Drogas, chkT3Alcohol, chkT3Violencia;

        // Vacunas completas (7 tipos)
        private CheckBox chkVacTetanos, chkVacTdpa, chkVacInfluenza, chkVacRubeola, chkVacHepB, chkVacHepA, chkVacCovid;
        private TextBox txtFechaTetanos, txtFechaTdpa, txtFechaInfluenza, txtFechaRubeola, txtFechaHepB, txtFechaHepA, txtFechaCovid;

        // Perfil Prenatal completo
        private TextBox txtCitoToma, txtCitoResFecha, txtCitoResultado, txtGrupoSanguineo;
        private TextBox txtHemoMenor, txtHemoMayor, txtChagas, txtBactMenor, txtBactMayor;
        private TextBox txtGlucemia, txtEstreptococo, txtObsVihMenor, txtObsVihMayor, txtObsSifMenor, txtObsSifMayor, txtUltraObs;
        private CheckBox chkVioVih, chkVioSifilis;

        // Solicitudes de Laboratorio Específicas (6 ítems)
        private CheckBox chkLabHb, chkLabSifilis, chkLabUrocultivo, chkLabVih, chkLabGlucosa, chkLabOrina;
        private TextBox txtFechaLabDefault;

        // Diagnóstico, Tipo de Consulta y Tratamiento
        private ComboBox cmbTipoDiagnostico;
        private ComboBox cmbTipoConsulta;
        private TextBox txtDiagnostico, txtReceta, txtRecomendaciones;
        private Button bGuardar;

        private void RegistroEmbarazadaForm_Load(object sender, EventArgs e)
        {
            // Lógica inicial del formulario al cargar, si se requiere
        }

        public RegistroEmbarazadaForm()
        {
            InitializeComponent();
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "Registro Especializado - Paciente Embarazada";
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
            lblTitulo.Text = "EXPEDIENTE CLÍNICO: PACIENTE EMBARAZADA";
            lblTitulo.Font = new Font("Arial", 11f, FontStyle.Bold);
            lblTitulo.Location = new Point(offsetX, y);
            lblTitulo.Size = new Size(anchoContenidoUtil, 25);
            panel.Controls.Add(lblTitulo);
            y += 40;

            // --- 1. DATOS GENERALES ---
            AgregarSeccion(panel, "1. Datos Generales de la Paciente", ref y);
            txtNombre = AgregarCampo(panel, "Nombre Completo:", ref y);
            txtExpediente = AgregarCampo(panel, "N° de Expediente:", ref y);
            txtEdad = AgregarCampo(panel, "Edad (años):", ref y);
            txtDireccion = AgregarCampo(panel, "Dirección:", ref y);
            txtResponsable = AgregarCampo(panel, "Acompañante / Responsable:", ref y);
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
            txtPeso = AgregarCampo(panel, "Peso Actual (kg):", ref y);
            txtTalla = AgregarCampo(panel, "Talla (cm):", ref y);
            txtCirAbdominal = AgregarCampo(panel, "Cir. Abdominal (cm):", ref y);
            y += 10;

            // --- 5. DATOS OBSTÉTRICOS Y GESTACIÓN ACTUAL ---
            AgregarSeccion(panel, "5. Datos Obstétricos y Gestación Actual", ref y);
            txtFormulaObstetrica = AgregarCampo(panel, "Fórmula Obstétrica (G:X P:X...):", ref y);
            txtFinEmbarazoAntFecha = AgregarCampo(panel, "Fin Embarazo Ant (Fecha/Año):", ref y);
            txtFinEmbarazoAntTerm = AgregarCampo(panel, "Terminación Embarazo Ant.:", ref y);
            chkEmbarazoNoDeseado = AgregarCheckbox(panel, "Embarazo no deseado", ref y);
            chkUsabaAnticonceptivo = AgregarCheckbox(panel, "Usaba método anticonceptivo al concebir", ref y);
            txtMetodoAnticonceptivoEmp = AgregarCampo(panel, "Método Anticonceptivo Empleado:", ref y);
            txtPesoAnterior = AgregarCampo(panel, "Peso Anterior/Inicial (kg):", ref y);
            txtEdadGestacional = AgregarCampo(panel, "Edad Gestacional (semanas):", ref y);
            dtpFUR = AgregarFechaCampo(panel, "Fecha Última Regla (FUR):", ref y);
            y += 10;

            // --- 6. USO DE ALCOHOL Y DROGAS POR TRIMESTRE ---
            AgregarSeccion(panel, "6. Uso de Alcohol, Drogas y Violencia (Por Trimestre)", ref y);

            Label lblT1 = new Label { Text = "Primer Trimestre:", Location = new Point(30, y), Size = new Size(200, 20), Font = new Font("Arial", 9, FontStyle.Bold) };
            panel.Controls.Add(lblT1); y += 24;
            chkT1FumaAct = AgregarCheckbox(panel, "Fuma activamente", ref y);
            chkT1FumaPas = AgregarCheckbox(panel, "Fuma pasivamente", ref y);
            chkT1Drogas = AgregarCheckbox(panel, "Drogas", ref y);
            chkT1Alcohol = AgregarCheckbox(panel, "Alcohol", ref y);
            chkT1Violencia = AgregarCheckbox(panel, "Violencia", ref y);

            Label lblT2 = new Label { Text = "Segundo Trimestre:", Location = new Point(30, y), Size = new Size(200, 20), Font = new Font("Arial", 9, FontStyle.Bold) };
            panel.Controls.Add(lblT2); y += 24;
            chkT2FumaAct = AgregarCheckbox(panel, "Fuma activamente", ref y);
            chkT2FumaPas = AgregarCheckbox(panel, "Fuma pasivamente", ref y);
            chkT2Drogas = AgregarCheckbox(panel, "Drogas", ref y);
            chkT2Alcohol = AgregarCheckbox(panel, "Alcohol", ref y);
            chkT2Violencia = AgregarCheckbox(panel, "Violencia", ref y);

            Label lblT3 = new Label { Text = "Tercer Trimestre:", Location = new Point(30, y), Size = new Size(200, 20), Font = new Font("Arial", 9, FontStyle.Bold) };
            panel.Controls.Add(lblT3); y += 24;
            chkT3FumaAct = AgregarCheckbox(panel, "Fuma activamente", ref y);
            chkT3FumaPas = AgregarCheckbox(panel, "Fuma pasivamente", ref y);
            chkT3Drogas = AgregarCheckbox(panel, "Drogas", ref y);
            chkT3Alcohol = AgregarCheckbox(panel, "Alcohol", ref y);
            chkT3Violencia = AgregarCheckbox(panel, "Violencia", ref y);
            y += 10;

            // --- 7. CARTILLA DE VACUNACIÓN  ---
            AgregarSeccion(panel, "7. Cartilla de Vacunación", ref y);
            chkVacTetanos = AgregarCheckbox(panel, "Vacuna Tétanos/difteria Aplicada", ref y);
            txtFechaTetanos = AgregarCampo(panel, "Fecha Aplicación Tétanos:", ref y);

            chkVacTdpa = AgregarCheckbox(panel, "Vacuna Tdpa Aplicada", ref y);
            txtFechaTdpa = AgregarCampo(panel, "Fecha Aplicación Tdpa:", ref y);

            chkVacInfluenza = AgregarCheckbox(panel, "Vacuna Influenza Aplicada", ref y);
            txtFechaInfluenza = AgregarCampo(panel, "Fecha Aplicación Influenza:", ref y);

            chkVacRubeola = AgregarCheckbox(panel, "Vacuna Rubéola Aplicada", ref y);
            txtFechaRubeola = AgregarCampo(panel, "Fecha Aplicación Rubéola:", ref y);

            chkVacHepB = AgregarCheckbox(panel, "Vacuna Hepatitis B Aplicada", ref y);
            txtFechaHepB = AgregarCampo(panel, "Fecha Aplicación Hep. B:", ref y);

            chkVacHepA = AgregarCheckbox(panel, "Vacuna Hepatitis A Aplicada", ref y);
            txtFechaHepA = AgregarCampo(panel, "Fecha Aplicación Hep. A:", ref y);

            chkVacCovid = AgregarCheckbox(panel, "Vacuna Covid Aplicada", ref y);
            txtFechaCovid = AgregarCampo(panel, "Fecha Aplicación Covid:", ref y);
            y += 10;

            // --- 8. PERFILES PRENATALES ---
            AgregarSeccion(panel, "8. Perfiles Prenatales y Laboratorios Base", ref y);
            txtCitoToma = AgregarCampo(panel, "Citología Fecha Toma:", ref y);
            txtCitoResFecha = AgregarCampo(panel, "Citología Fecha Resultado:", ref y);
            txtCitoResultado = AgregarCampo(panel, "Citología Resultado:", ref y);
            txtGrupoSanguineo = AgregarCampo(panel, "Grupo Sanguíneo:", ref y);
            txtHemoMenor = AgregarCampo(panel, "Hemoglobina (<20 sem):", ref y);
            txtHemoMayor = AgregarCampo(panel, "Hemoglobina (>=20 sem):", ref y);
            txtChagas = AgregarCampo(panel, "Chagas:", ref y);
            txtBactMenor = AgregarCampo(panel, "Bacteriuria (<20 sem):", ref y);
            txtBactMayor = AgregarCampo(panel, "Bacteriuria (>=20 sem):", ref y);
            txtGlucemia = AgregarCampo(panel, "Glucemia Ayuno (<30 sem):", ref y);
            txtEstreptococo = AgregarCampo(panel, "Estreptococos B (30-37 sem):", ref y);

            chkVioVih = AgregarCheckbox(panel, "¿Se realizó prueba de VIH?", ref y);
            txtObsVihMenor = AgregarCampo(panel, "VIH - Observaciones <20 sem:", ref y);
            txtObsVihMayor = AgregarCampo(panel, "VIH - Observaciones >=20 sem:", ref y);

            chkVioSifilis = AgregarCheckbox(panel, "¿Se realizó prueba de Sífilis?", ref y);
            txtObsSifMenor = AgregarCampo(panel, "Sífilis - Observaciones <20 sem:", ref y);
            txtObsSifMayor = AgregarCampo(panel, "Sífilis - Observaciones >=20 sem:", ref y);

            txtUltraObs = AgregarCampo(panel, "Ultrasonografía Obs:", ref y);
            y += 10;

            // --- 9. SOLICITUD DE LABORATORIOS ESPECÍFICOS ---
            AgregarSeccion(panel, "9. Solicitud de Laboratorios Específicos", ref y);
            txtFechaLabDefault = AgregarCampo(panel, "Fecha para Realizar Exámenes:", ref y);
            txtFechaLabDefault.Text = DateTime.Now.ToShortDateString();

            chkLabHb = AgregarCheckbox(panel, "Solicitar Examen: HT - HB", ref y);
            chkLabSifilis = AgregarCheckbox(panel, "Solicitar Examen: Serología de sífilis", ref y);
            chkLabUrocultivo = AgregarCheckbox(panel, "Solicitar Examen: Urocultivo", ref y);
            chkLabVih = AgregarCheckbox(panel, "Solicitar Examen: VIH", ref y);
            chkLabGlucosa = AgregarCheckbox(panel, "Solicitar Examen: Glucosa", ref y);
            chkLabOrina = AgregarCheckbox(panel, "Solicitar Examen: General de orina", ref y);
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
            bGuardar.Text = "Guardar Expediente Embarazada";
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
            chk.Size = new Size(480, 24);
            panel.Controls.Add(chk);
            y += 28;
            return chk;
        }

        private DateTimePicker AgregarFechaCampo(Panel panel, string etiqueta, ref int y)
        {
            Label lbl = new Label();
            lbl.Text = etiqueta;
            lbl.Location = new Point(30, y);
            lbl.Size = new Size(210, 20);
            panel.Controls.Add(lbl);

            DateTimePicker dtp = new DateTimePicker();
            dtp.Location = new Point(250, y);
            dtp.Size = new Size(300, 23);
            dtp.Format = DateTimePickerFormat.Short;
            panel.Controls.Add(dtp);

            y += 32;
            return dtp;
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

                if (!int.TryParse(txtEdad.Text, out int edad) || edad < 10 || edad > 55)
                {
                    MessageBox.Show("Para paciente embarazada, la edad debe estar entre 10 y 55 años.", "Error de Rango", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Instanciación de PacienteEmbarazada
                PacienteEmbarazada paciente = new PacienteEmbarazada(
                    txtNombre.Text.Trim(),
                    edad,
                    txtExpediente.Text.Trim(),
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

                // Signos vitales
                double.TryParse(txtTemp.Text, out double temp);
                int.TryParse(txtFR.Text, out int fr);
                int.TryParse(txtFC.Text, out int fc);
                int.TryParse(txtPulso.Text, out int pulso);
                double.TryParse(txtSpO2.Text, out double spo2);
                double.TryParse(txtPeso.Text, out double peso);
                double.TryParse(txtTalla.Text, out double talla);
                double.TryParse(txtCirAbdominal.Text, out double cir);
                double.TryParse(txtPesoAnterior.Text, out double pesoAnt);

                paciente.TemperaturaCelsius = temp;
                paciente.FrecuenciaRespiratoria = fr;
                paciente.FrecuenciaCardiaca = fc;
                paciente.Pulso = pulso;
                paciente.PresionArterial = string.IsNullOrWhiteSpace(txtPA.Text) ? "120/80" : txtPA.Text.Trim();
                paciente.SaturacionOxigeno = spo2;
                paciente.PesoKg = peso;
                paciente.TallaCm = talla;
                paciente.CircunferenciaAbdominalCm = cir;

                // Datos obstétricos
                paciente.FormulaObstetrica = string.IsNullOrWhiteSpace(txtFormulaObstetrica.Text) ? "G1 P0 A0 V0" : txtFormulaObstetrica.Text.Trim();
                paciente.FinEmbarazoAnteriorFecha = txtFinEmbarazoAntFecha.Text.Trim();
                paciente.FinEmbarazoAnteriorTerminacion = txtFinEmbarazoAntTerm.Text.Trim();
                paciente.EmbarazoNoDeseado = chkEmbarazoNoDeseado.Checked;
                paciente.UsabaMetodoAnticonceptivo = chkUsabaAnticonceptivo.Checked;
                paciente.MetodoAnticonceptivoEmpleado = txtMetodoAnticonceptivoEmp.Text.Trim();
                paciente.PesoAnteriorKg = pesoAnt;
                paciente.EdadGestacionalTexto = txtEdadGestacional.Text.Trim();

                paciente.FUR = dtpFUR.Value;
                paciente.CalcularFPP();

                // Consumo y riesgos por trimestre
                paciente.Trimestre1 = new ConsumoYRiesgoTrimestre { FumaActivamente = chkT1FumaAct.Checked, FumaPasivamente = chkT1FumaPas.Checked, Drogas = chkT1Drogas.Checked, Alcohol = chkT1Alcohol.Checked, Violencia = chkT1Violencia.Checked };
                paciente.Trimestre2 = new ConsumoYRiesgoTrimestre { FumaActivamente = chkT2FumaAct.Checked, FumaPasivamente = chkT2FumaPas.Checked, Drogas = chkT2Drogas.Checked, Alcohol = chkT2Alcohol.Checked, Violencia = chkT2Violencia.Checked };
                paciente.Trimestre3 = new ConsumoYRiesgoTrimestre { FumaActivamente = chkT3FumaAct.Checked, FumaPasivamente = chkT3FumaPas.Checked, Drogas = chkT3Drogas.Checked, Alcohol = chkT3Alcohol.Checked, Violencia = chkT3Violencia.Checked };

                // Vacunas 
                if (chkVacTetanos.Checked) paciente.Vacunas.Add(new RegistroVacuna { NombreVacuna = "Tétanos/difteria", Aplicada = true, FechaAplicacion = txtFechaTetanos.Text.Trim() });
                if (chkVacTdpa.Checked) paciente.Vacunas.Add(new RegistroVacuna { NombreVacuna = "Tdpa", Aplicada = true, FechaAplicacion = txtFechaTdpa.Text.Trim() });
                if (chkVacInfluenza.Checked) paciente.Vacunas.Add(new RegistroVacuna { NombreVacuna = "Influenza", Aplicada = true, FechaAplicacion = txtFechaInfluenza.Text.Trim() });
                if (chkVacRubeola.Checked) paciente.Vacunas.Add(new RegistroVacuna { NombreVacuna = "Rubéola", Aplicada = true, FechaAplicacion = txtFechaRubeola.Text.Trim() });
                if (chkVacHepB.Checked) paciente.Vacunas.Add(new RegistroVacuna { NombreVacuna = "Hepatitis B", Aplicada = true, FechaAplicacion = txtFechaHepB.Text.Trim() });
                if (chkVacHepA.Checked) paciente.Vacunas.Add(new RegistroVacuna { NombreVacuna = "Hepatitis A", Aplicada = true, FechaAplicacion = txtFechaHepA.Text.Trim() });
                if (chkVacCovid.Checked) paciente.Vacunas.Add(new RegistroVacuna { NombreVacuna = "Covid", Aplicada = true, FechaAplicacion = txtFechaCovid.Text.Trim() });

                // Perfil Prenatal completo
                string vihRes = chkVioVih.Checked ? $"Realizada. <20sem: {txtObsVihMenor.Text.Trim()} | >=20sem: {txtObsVihMayor.Text.Trim()}" : "No se realizó";
                string sifilisRes = chkVioSifilis.Checked ? $"Realizada. <20sem: {txtObsSifMenor.Text.Trim()} | >=20sem: {txtObsSifMayor.Text.Trim()}" : "No se realizó";

                paciente.Prenatal = new PerfilPrenatal
                {
                    CitologiaFechaToma = txtCitoToma.Text.Trim(),
                    CitologiaFechaResultado = txtCitoResFecha.Text.Trim(),
                    CitologiaResultado = txtCitoResultado.Text.Trim(),
                    GrupoSanguineo = txtGrupoSanguineo.Text.Trim(),
                    HemoglobinaMenor20Sem = txtHemoMenor.Text.Trim(),
                    HemoglobinaMayor20Sem = txtHemoMayor.Text.Trim(),
                    Chagas = txtChagas.Text.Trim(),
                    BacteriuriaMenor20Sem = txtBactMenor.Text.Trim(),
                    BacteriuriaMayor20Sem = txtBactMayor.Text.Trim(),
                    GlucemiaAyunoMenor30Sem = txtGlucemia.Text.Trim(),
                    EstreptococosB_30_37Sem = txtEstreptococo.Text.Trim(),
                    VIH = vihRes,
                    Sifilis = sifilisRes,
                    UltrasonografiaObs = txtUltraObs.Text.Trim()
                };

                // Solicitud de laboratorios específicos 
                string fechaLab = string.IsNullOrWhiteSpace(txtFechaLabDefault.Text) ? DateTime.Now.ToShortDateString() : txtFechaLabDefault.Text.Trim();

                if (chkLabHb.Checked) paciente.LaboratoriosEmbarazada.Add(new SolicitudLaboratorioEmbarazada { Fecha = fechaLab, NombreExamen = "HT - HB", TipoMuestra = "sangre" });
                if (chkLabSifilis.Checked) paciente.LaboratoriosEmbarazada.Add(new SolicitudLaboratorioEmbarazada { Fecha = fechaLab, NombreExamen = "Serología de sífilis", TipoMuestra = "sangre" });
                if (chkLabUrocultivo.Checked) paciente.LaboratoriosEmbarazada.Add(new SolicitudLaboratorioEmbarazada { Fecha = fechaLab, NombreExamen = "Urocultivo", TipoMuestra = "orina" });
                if (chkLabVih.Checked) paciente.LaboratoriosEmbarazada.Add(new SolicitudLaboratorioEmbarazada { Fecha = fechaLab, NombreExamen = "VIH", TipoMuestra = "sangre" });
                if (chkLabGlucosa.Checked) paciente.LaboratoriosEmbarazada.Add(new SolicitudLaboratorioEmbarazada { Fecha = fechaLab, NombreExamen = "Glucosa", TipoMuestra = "sangre" });
                if (chkLabOrina.Checked) paciente.LaboratoriosEmbarazada.Add(new SolicitudLaboratorioEmbarazada { Fecha = fechaLab, NombreExamen = "General de orina", TipoMuestra = "orina" });

                // Diagnóstico, tipo de consulta y tratamiento
                paciente.DiagnosticoTexto = txtDiagnostico.Text.Trim();
                paciente.TipoDiagnostico = cmbTipoDiagnostico.SelectedItem.ToString();
                paciente.TipoConsulta = cmbTipoConsulta.SelectedItem.ToString();
                paciente.Recomendaciones = txtRecomendaciones.Text.Trim();

                if (!string.IsNullOrWhiteSpace(txtReceta.Text))
                {
                    paciente.Receta.Add(new Medicamento("Suplemento / Medicamento Prenatal", txtReceta.Text.Trim()));
                }

                // Guardar en base de datos mediante Entity Framework
                repositorio.RegistrarPaciente(paciente);

                MessageBox.Show($"¡Expediente de Paciente Embarazada guardado con éxito!\nFPP Calculada: {paciente.FPP.ToShortDateString()}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al guardar el registro: {ex.Message}", "Error de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
