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
    public partial class RegistroPacienteForm : Form
    {
        private IRepositorioPaciente repositorio = new RepositorioSql();

        private Label lblTitulo;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblExpediente;
        private TextBox txtExpediente;
        private Label lblEdad;
        private TextBox txtEdad;
        private Label lblSexo;
        private ComboBox cmbSexo;
        private Label lblTipoPaciente;
        private ComboBox cmbTipoPaciente;

        // Campo dinámico secundario
        private Label lblDatoAdicional;
        private TextBox txtDatoAdicional;

        private Button btnGuardar;

        public RegistroPacienteForm()
        {
            InitializeComponent();
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "Registro Especializado de Paciente - Unidad de Salud";
            this.Size = new Size(420, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Título
            lblTitulo = new Label();
            lblTitulo.Text = "REGISTRO CLÍNICO AVANZADO";
            lblTitulo.Font = new Font("Arial", 12, FontStyle.Bold);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            lblTitulo.Location = new Point(30, 20);
            lblTitulo.Size = new Size(340, 25);
            this.Controls.Add(lblTitulo);

            // Nombre
            lblNombre = new Label();
            lblNombre.Text = "Nombre Completo:";
            lblNombre.Location = new Point(40, 65);
            lblNombre.Size = new Size(150, 20);
            this.Controls.Add(lblNombre);

            txtNombre = new TextBox();
            txtNombre.Location = new Point(40, 90);
            txtNombre.Size = new Size(320, 25);
            this.Controls.Add(txtNombre);

            // Número de Expediente
            lblExpediente = new Label();
            lblExpediente.Text = "N° de Expediente:";
            lblExpediente.Location = new Point(40, 130);
            lblExpediente.Size = new Size(150, 20);
            this.Controls.Add(lblExpediente);

            txtExpediente = new TextBox();
            txtExpediente.Location = new Point(40, 155);
            txtExpediente.Size = new Size(320, 25);
            this.Controls.Add(txtExpediente);

            // Edad
            lblEdad = new Label();
            lblEdad.Text = "Edad:";
            lblEdad.Location = new Point(40, 195);
            lblEdad.Size = new Size(150, 20);
            this.Controls.Add(lblEdad);

            txtEdad = new TextBox();
            txtEdad.Location = new Point(40, 220);
            txtEdad.Size = new Size(320, 25);
            this.Controls.Add(txtEdad);

            // Sexo
            lblSexo = new Label();
            lblSexo.Text = "Sexo:";
            lblSexo.Location = new Point(40, 260);
            lblSexo.Size = new Size(150, 20);
            this.Controls.Add(lblSexo);

            cmbSexo = new ComboBox();
            cmbSexo.Location = new Point(40, 285);
            cmbSexo.Size = new Size(320, 25);
            cmbSexo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSexo.Items.AddRange(new object[] { "Masculino", "Femenino" });
            cmbSexo.SelectedIndex = 0;
            this.Controls.Add(cmbSexo);

            // Tipo de Paciente (Subclase)
            lblTipoPaciente = new Label();
            lblTipoPaciente.Text = "Perfil Clínico (Subclase):";
            lblTipoPaciente.Location = new Point(40, 325);
            lblTipoPaciente.Size = new Size(180, 20);
            this.Controls.Add(lblTipoPaciente);

            cmbTipoPaciente = new ComboBox();
            cmbTipoPaciente.Location = new Point(40, 350);
            cmbTipoPaciente.Size = new Size(320, 25);
            cmbTipoPaciente.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipoPaciente.Items.AddRange(new object[] {
                "Paciente Infantil",
                "Paciente Adolescente",
                "Paciente Adulto Mayor",
                "Paciente Crónico",
                "Paciente Embarazada"
            });
            cmbTipoPaciente.SelectedIndex = 0;
            cmbTipoPaciente.SelectedIndexChanged += CmbTipoPaciente_SelectedIndexChanged;
            this.Controls.Add(cmbTipoPaciente);

            // Dato Adicional (Responsable)
            lblDatoAdicional = new Label();
            lblDatoAdicional.Text = "Responsable / Tutor:";
            lblDatoAdicional.Location = new Point(40, 390);
            lblDatoAdicional.Size = new Size(200, 20);
            this.Controls.Add(lblDatoAdicional);

            txtDatoAdicional = new TextBox();
            txtDatoAdicional.Location = new Point(40, 415);
            txtDatoAdicional.Size = new Size(320, 25);
            this.Controls.Add(txtDatoAdicional);

            // Botón Guardar
            btnGuardar = new Button();
            btnGuardar.Text = "Guardar Expediente Polimórfico";
            btnGuardar.Location = new Point(40, 455);
            btnGuardar.Size = new Size(320, 35);
            btnGuardar.BackColor = Color.FromArgb(0, 122, 204);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Font = new Font("Arial", 9, FontStyle.Bold);
            btnGuardar.Click += BtnGuardar_Click;
            this.Controls.Add(btnGuardar);
        }

        private void CmbTipoPaciente_SelectedIndexChanged(object sender, EventArgs e)
        {
            string seleccionado = cmbTipoPaciente.SelectedItem.ToString();
            switch (seleccionado)
            {
                case "Paciente Infantil":
                    lblDatoAdicional.Text = "Nombre del Tutor:";
                    break;
                case "Paciente Adolescente":
                    lblDatoAdicional.Text = "Responsable / Familiar:";
                    break;
                case "Paciente Adulto Mayor":
                    lblDatoAdicional.Text = "Persona a Cargo:";
                    break;
                case "Paciente Crónico":
                    lblDatoAdicional.Text = "Familiar Responsable:";
                    break;
                case "Paciente Embarazada":
                    lblDatoAdicional.Text = "Nombre del Responsable:";
                    break;
            }
        }

        // Método requerido por el Diseñador para evitar el error CS1061
        private void RegistroPacienteForm_Load(object sender, EventArgs e)
        {
            // Inicialización al cargar el formulario
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txtNombre.Text.Trim();
                string expediente = txtExpediente.Text.Trim();
                string responsable = txtDatoAdicional.Text.Trim();

                if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(expediente))
                {
                    MessageBox.Show("Por favor, complete los campos obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtEdad.Text.Trim(), out int edad))
                {
                    MessageBox.Show("La edad debe ser un número entero válido.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string sexo = cmbSexo.SelectedItem.ToString();
                string tipo = cmbTipoPaciente.SelectedItem.ToString();
                string direccion = "Dirección estándar";
                if (string.IsNullOrEmpty(responsable)) responsable = "N/A";

                // Instanciación polimórfica respetando exactamente los constructores de tus clases:
                // (string nom, int ed, string exp, string sx, string dir, string resp)
                Paciente nuevoPaciente;
                switch (tipo)
                {
                    case "Paciente Infantil":
                        nuevoPaciente = new PacienteInfantil(nombre, edad, expediente, sexo, direccion, responsable);
                        break;
                    case "Paciente Adolescente":
                        nuevoPaciente = new PacienteAdolescente(nombre, edad, expediente, sexo, direccion, responsable);
                        break;
                    case "Paciente Adulto Mayor":
                        nuevoPaciente = new PacienteAdultoMayor(nombre, edad, expediente, sexo, direccion, responsable);
                        break;
                    case "Paciente Crónico":
                        nuevoPaciente = new PacienteCronico(nombre, edad, expediente, sexo, direccion, responsable);
                        break;
                    default:
                        nuevoPaciente = new PacienteEmbarazada(nombre, edad, expediente, direccion, responsable);
                        break;
                }

                // Guardado formal mediante el Patrón Repository hacia la BD SQLite
                repositorio.RegistrarPaciente(nuevoPaciente);

                MessageBox.Show($"¡Expediente polimórfico guardado con éxito!\nTipo: {tipo}\nExpediente N°: {expediente}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al registrar el paciente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
