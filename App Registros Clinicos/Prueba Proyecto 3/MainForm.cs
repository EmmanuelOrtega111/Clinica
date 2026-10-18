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

    public partial class MainForm : Form
    {
        // Componentes del formulario
        private Button btnRegistrarPaciente;
        private Button btnVerPacientes;
        private Button btnCerrarSesion;
        private Label lblBienvenida;
        private Label lblTitulo;
        private Panel panelContenido;

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Lógica inicial del formulario al cargar, si se requiere
        }
        public MainForm()
        {
            InitializeComponent();
            ConfigurarVentanaPrincipal();
        }

        private void ConfigurarVentanaPrincipal()
        {
            // Configuración básica de la ventana
            this.Text = "Panel Principal - Gestión de Expedientes Clínicos";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Título Principal
            lblTitulo = new Label();
            lblTitulo.Text = "REGISTRO CLÍNICO";
            lblTitulo.Font = new Font("Arial", 14, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(0, 102, 204);
            lblTitulo.Location = new Point(30, 20);
            lblTitulo.Size = new Size(600, 30);
            this.Controls.Add(lblTitulo);

            // Mensaje de bienvenida
            lblBienvenida = new Label();
            lblBienvenida.Text = "Bienvenido/a al panel de administración de pacientes.";
            lblBienvenida.Font = new Font("Arial", 10, FontStyle.Regular);
            lblBienvenida.Location = new Point(30, 55);
            lblBienvenida.Size = new Size(500, 25);
            this.Controls.Add(lblBienvenida);

            // --- BOTONES PRINCIPALES ---

            // 1. Botón Registrar Paciente (Conectado al nuevo Selector)
            btnRegistrarPaciente = new Button();
            btnRegistrarPaciente.Text = "Registrar Paciente";
            btnRegistrarPaciente.Font = new Font("Arial", 10, FontStyle.Bold);
            btnRegistrarPaciente.BackColor = Color.FromArgb(0, 122, 204);
            btnRegistrarPaciente.ForeColor = Color.White;
            btnRegistrarPaciente.Location = new Point(30, 95);
            btnRegistrarPaciente.Size = new Size(180, 40);
            btnRegistrarPaciente.FlatStyle = FlatStyle.Flat;
            btnRegistrarPaciente.FlatAppearance.BorderSize = 0;
          
            btnRegistrarPaciente.Click += btnRegistrarPaciente_Click;
            this.Controls.Add(btnRegistrarPaciente);

            // 2. Botón Ver Pacientes / Historial
            btnVerPacientes = new Button();
            btnVerPacientes.Text = "Ver Pacientes";
            btnVerPacientes.Font = new Font("Arial", 10, FontStyle.Bold);
            btnVerPacientes.BackColor = Color.FromArgb(40, 167, 69);
            btnVerPacientes.ForeColor = Color.White;
            btnVerPacientes.Location = new Point(220, 95);
            btnVerPacientes.Size = new Size(180, 40);
            btnVerPacientes.FlatStyle = FlatStyle.Flat;
            btnVerPacientes.FlatAppearance.BorderSize = 0;
            btnVerPacientes.Click += btnVerPacientes_Click;
            this.Controls.Add(btnVerPacientes);

            // 3. Botón Cerrar Sesión
            btnCerrarSesion = new Button();
            btnCerrarSesion.Text = "Cerrar Sesión";
            btnCerrarSesion.Font = new Font("Arial", 10, FontStyle.Bold);
            btnCerrarSesion.BackColor = Color.FromArgb(220, 53, 69);
            btnCerrarSesion.ForeColor = Color.White;
            btnCerrarSesion.Location = new Point(570, 95);
            btnCerrarSesion.Size = new Size(170, 40);
            btnCerrarSesion.FlatStyle = FlatStyle.Flat;
            btnCerrarSesion.FlatAppearance.BorderSize = 0;
            btnCerrarSesion.Click += btnCerrarSesion_Click;
            this.Controls.Add(btnCerrarSesion);

            // Panel contenedor central para mostrar tablas o información
            panelContenido = new Panel();
            panelContenido.Location = new Point(30, 155);
            panelContenido.Size = new Size(710, 280);
            panelContenido.BorderStyle = BorderStyle.FixedSingle;
            panelContenido.BackColor = Color.White;
            this.Controls.Add(panelContenido);
        }

        // --- EVENTOS DE LOS BOTONES ---

        private void btnRegistrarPaciente_Click(object sender, EventArgs e)
        {
            // Abre de forma modal el selector de perfiles clínicos especializados
            SeleccionTipoPacienteForm selector = new SeleccionTipoPacienteForm();
            selector.ShowDialog();
        }

        private void btnVerPacientes_Click(object sender, EventArgs e)
        {
            panelContenido.Controls.Clear();

            DataGridView dgvPacientes = new DataGridView();
            dgvPacientes.Dock = DockStyle.Fill;
            dgvPacientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPacientes.ReadOnly = true;
            dgvPacientes.AllowUserToAddRows = false;
            dgvPacientes.RowHeadersVisible = false;

            // Consultar directamente con la entidad del ADO.NET EDMX
            using (var context = new Prueba_Proyecto_3.Modelo.SistemaUnidadSaludEntities())
            {
                var listaPacientes = context.Pacientes.Select(p => new
                {
                    Expediente = p.NumeroExpediente,
                    Nombre = p.Nombre,
                    Edad = p.Edad,
                    Sexo = p.Sexo,
                    Tipo = p.TipoPaciente,
                    Direccion = p.Direccion,
                    Responsable = p.Responsable
                }).ToList();

                dgvPacientes.DataSource = listaPacientes;
            }

            panelContenido.Controls.Add(dgvPacientes);
        }
        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("¿Está seguro de que desea cerrar la sesión actual?", "Cerrar Sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
