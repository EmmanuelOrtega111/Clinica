using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using SistemaUnidadSalud.Data;

namespace Prueba_Proyecto_3
{
    public partial class LoginForm : Form
    {
        public static bool SesionCerradaParaReinicio { get; set; } = false;

        private Label lblTitulo;
        private Label lblUsuario;
        private TextBox txtUsuario;
        private Label lblContra;
        private TextBox txtContra;
        private Button btnIngresar;

        public LoginForm()
        {
            InitializeComponent();
            ConfigurarVentana();
        }

        private void ConfigurarVentana()
        {
            this.Text = "Unidad de Salud - Autenticación";
            this.Size = new Size(400, 320);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Título principal
            lblTitulo = new Label();
            lblTitulo.Text = "CONTROL DE ACCESO";
            lblTitulo.Font = new Font("Arial", 14, FontStyle.Bold);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            lblTitulo.Location = new Point(30, 20);
            lblTitulo.Size = new Size(320, 30);
            this.Controls.Add(lblTitulo);

            // Campo de Usuario
            lblUsuario = new Label();
            lblUsuario.Text = "Usuario:";
            lblUsuario.Location = new Point(40, 65);
            lblUsuario.Size = new Size(100, 20);
            this.Controls.Add(lblUsuario);

            txtUsuario = new TextBox();
            txtUsuario.Location = new Point(40, 85);
            txtUsuario.Size = new Size(300, 25);
            this.Controls.Add(txtUsuario);

            // Campo de Contraseña
            lblContra = new Label();
            lblContra.Text = "Contraseña:";
            lblContra.Location = new Point(40, 125);
            lblContra.Size = new Size(100, 20);
            this.Controls.Add(lblContra);

            txtContra = new TextBox();
            txtContra.Location = new Point(40, 145);
            txtContra.Size = new Size(300, 25);
            txtContra.UseSystemPasswordChar = true;
            this.Controls.Add(txtContra);

            // Botón de Ingresar
            btnIngresar = new Button();
            btnIngresar.Text = "Iniciar Sesión";
            btnIngresar.Location = new Point(40, 195);
            btnIngresar.Size = new Size(300, 35);
            btnIngresar.BackColor = Color.FromArgb(0, 122, 204);
            btnIngresar.ForeColor = Color.White;
            btnIngresar.Font = new Font("Arial", 10, FontStyle.Bold);
            btnIngresar.Click += BtnIngresar_Click;
            this.Controls.Add(btnIngresar);
        }

        private void BtnIngresar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string password = txtContra.Text.Trim();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Por favor, ingrese el usuario y la contraseña.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ConexionBD conexionBD = new ConexionBD();
            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            {
                try
                {
                    conexion.Open();
                    string query = "SELECT Rol FROM Usuarios WHERE NombreUs = @usuario AND contra = @contra";
                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        cmd.Parameters.AddWithValue("@contra", password);

                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            string rolObtenido = result.ToString();

                            // Guardar datos de sesión
                            SesionUsuario.Instancia.IniciarSesion(usuario, rolObtenido);

                            MessageBox.Show($"Bienvenido/a, {usuario} [{rolObtenido}]", "Sesión Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Marcamos OK y cerramos este diálogo
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Usuario o contraseña incorrectos.", "Error de Autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al conectar con la base de datos: {ex.Message}", "Error de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}