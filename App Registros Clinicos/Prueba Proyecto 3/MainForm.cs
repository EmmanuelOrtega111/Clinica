using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Prueba_Proyecto_3
{
    public partial class MainForm : Form
    {
        // Componentes del formulario
        private Button btnRegistrarPaciente;
        private Button btnVerPacientes;
        private Button btnGestionarUsuarios; // Nuevo botón para Administrador
        private Button btnCerrarSesion;
        private Label lblBienvenida;
        private Label lblTitulo;
        private Panel panelContenido;

        public MainForm()
        {
            InitializeComponent();
            ConfigurarVentanaPrincipal();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Opciones de inicialización al cargar la ventana
        }

        private void ConfigurarVentanaPrincipal()
        {
            // Configuración básica de la ventana
            this.Text = "Panel Principal - Gestión de Expedientes Clínicos";
            this.Size = new Size(850, 500);
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

            // Mensaje de bienvenida adaptativo
            lblBienvenida = new Label();
            lblBienvenida.Text = $"Bienvenido/a {SesionUsuario.Instancia.NombreUsuario} [{SesionUsuario.Instancia.Rol}] al panel de gestión.";
            lblBienvenida.Font = new Font("Arial", 10, FontStyle.Regular);
            lblBienvenida.Location = new Point(30, 55);
            lblBienvenida.Size = new Size(600, 25);
            this.Controls.Add(lblBienvenida);

            // --- BOTONES PRINCIPALES ---

            // 1. Botón Registrar Paciente
            btnRegistrarPaciente = new Button();
            btnRegistrarPaciente.Text = "Registrar Paciente";
            btnRegistrarPaciente.Font = new Font("Arial", 9, FontStyle.Bold);
            btnRegistrarPaciente.BackColor = Color.FromArgb(0, 122, 204);
            btnRegistrarPaciente.ForeColor = Color.White;
            btnRegistrarPaciente.Location = new Point(30, 95);
            btnRegistrarPaciente.Size = new Size(160, 40);
            btnRegistrarPaciente.FlatStyle = FlatStyle.Flat;
            btnRegistrarPaciente.FlatAppearance.BorderSize = 0;
            btnRegistrarPaciente.Click += btnRegistrarPaciente_Click;
            this.Controls.Add(btnRegistrarPaciente);

            // 2. Botón Ver Pacientes / Historial
            btnVerPacientes = new Button();
            btnVerPacientes.Text = "Ver Pacientes";
            btnVerPacientes.Font = new Font("Arial", 9, FontStyle.Bold);
            btnVerPacientes.BackColor = Color.FromArgb(40, 167, 69);
            btnVerPacientes.ForeColor = Color.White;
            btnVerPacientes.Location = new Point(200, 95);
            btnVerPacientes.Size = new Size(160, 40);
            btnVerPacientes.FlatStyle = FlatStyle.Flat;
            btnVerPacientes.FlatAppearance.BorderSize = 0;
            btnVerPacientes.Click += btnVerPacientes_Click;
            this.Controls.Add(btnVerPacientes);

            // 3. Botón Gestionar Usuarios (CONDICIONAL: Solo si es Administrador)
            if (SesionUsuario.Instancia.Rol != null &&
                SesionUsuario.Instancia.Rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase))
            {
                btnGestionarUsuarios = new Button();
                btnGestionarUsuarios.Text = "Gestionar Usuarios";
                btnGestionarUsuarios.Font = new Font("Arial", 9, FontStyle.Bold);
                btnGestionarUsuarios.BackColor = Color.FromArgb(255, 193, 7);
                btnGestionarUsuarios.ForeColor = Color.Black;
                btnGestionarUsuarios.Location = new Point(370, 95);
                btnGestionarUsuarios.Size = new Size(160, 40);
                btnGestionarUsuarios.FlatStyle = FlatStyle.Flat;
                btnGestionarUsuarios.FlatAppearance.BorderSize = 0;
                btnGestionarUsuarios.Click += btnGestionarUsuarios_Click;
                this.Controls.Add(btnGestionarUsuarios);
            }

            // 4. Botón Cerrar Sesión
            btnCerrarSesion = new Button();
            btnCerrarSesion.Text = "Cerrar Sesión";
            btnCerrarSesion.Font = new Font("Arial", 9, FontStyle.Bold);
            btnCerrarSesion.BackColor = Color.FromArgb(220, 53, 69);
            btnCerrarSesion.ForeColor = Color.White;
            btnCerrarSesion.Location = new Point(650, 95);
            btnCerrarSesion.Size = new Size(140, 40);
            btnCerrarSesion.FlatStyle = FlatStyle.Flat;
            btnCerrarSesion.FlatAppearance.BorderSize = 0;
            btnCerrarSesion.Click += btnCerrarSesion_Click;
            this.Controls.Add(btnCerrarSesion);

            // Panel contenedor central
            panelContenido = new Panel();
            panelContenido.Location = new Point(30, 155);
            panelContenido.Size = new Size(760, 280);
            panelContenido.BorderStyle = BorderStyle.FixedSingle;
            panelContenido.BackColor = Color.White;
            this.Controls.Add(panelContenido);
        }

        // --- EVENTOS DE LOS BOTONES ---

        private void btnRegistrarPaciente_Click(object sender, EventArgs e)
        {
            SeleccionTipoPacienteForm selector = new SeleccionTipoPacienteForm();
            selector.ShowDialog();
        }

        private void btnVerPacientes_Click(object sender, EventArgs e)
        {
            panelContenido.Controls.Clear();

            DataGridView dgvPacientes = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false
            };

            SistemaUnidadSalud.Data.ConexionBD conexionBD = new SistemaUnidadSalud.Data.ConexionBD();
            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            {
                string query = @"
                    SELECT 
                        NumeroExpediente AS Expediente,
                        Nombre,
                        Edad,
                        Sexo,
                        TipoPaciente AS Tipo,
                        Direccion,
                        Responsable
                    FROM Pacientes";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conexion);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvPacientes.DataSource = dt;
            }

            panelContenido.Controls.Add(dgvPacientes);
        }

        private void btnGestionarUsuarios_Click(object sender, EventArgs e)
        {
            GestionUsers frmGestion = new GestionUsers();
            frmGestion.ShowDialog();
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("¿Está seguro de que desea cerrar la sesión actual?", "Cerrar Sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                // Indicar que fue un Cierre de Sesión intencional para reiniciar en el Login
                LoginForm.SesionCerradaParaReinicio = true;
                SesionUsuario.Instancia.CerrarSesion();
                this.Close();
            }
        }
    }
}