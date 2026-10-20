using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using SistemaUnidadSalud.Data;

namespace Prueba_Proyecto_3
{
    public partial class editUsers : Form
    {
        private GestionUsers _padre;
        private int _idUsuario;

        private Label lblTitulo;
        private Label lblUsuario;
        private TextBox txtUsuario;
        private Label lblContra;
        private TextBox txtContra;
        private Label lblRol;
        private ComboBox cmbRol;
        private Button btnGuardar;
        private Button btnCancelar;

        public editUsers(GestionUsers padre, int idUsuario, string usuario, string contra, string rol)
        {
            InitializeComponent();
            _padre = padre;
            _idUsuario = idUsuario;
            ConfigurarVentana();

            txtUsuario.Text = usuario;
            txtContra.Text = contra;
            cmbRol.SelectedItem = rol;
        }

        private void ConfigurarVentana()
        {
            this.Text = "Editar Usuario";
            this.Size = new Size(380, 320);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            lblTitulo = new Label();
            lblTitulo.Text = "EDITAR USUARIO";
            lblTitulo.Font = new Font("Arial", 12, FontStyle.Bold);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            lblTitulo.Location = new Point(30, 15);
            lblTitulo.Size = new Size(300, 25);
            this.Controls.Add(lblTitulo);

            lblUsuario = new Label { Text = "Nombre de Usuario:", Location = new Point(30, 55), Size = new Size(150, 20) };
            this.Controls.Add(lblUsuario);

            txtUsuario = new TextBox { Location = new Point(30, 75), Size = new Size(300, 25) };
            this.Controls.Add(txtUsuario);

            lblContra = new Label { Text = "Contraseña:", Location = new Point(30, 115), Size = new Size(150, 20) };
            this.Controls.Add(lblContra);

            txtContra = new TextBox { Location = new Point(30, 135), Size = new Size(300, 25), UseSystemPasswordChar = true };
            this.Controls.Add(txtContra);

            lblRol = new Label { Text = "Rol:", Location = new Point(30, 175), Size = new Size(150, 20) };
            this.Controls.Add(lblRol);

            cmbRol = new ComboBox { Location = new Point(30, 195), Size = new Size(300, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbRol.Items.AddRange(new object[] { "Administrador", "Médico" });
            this.Controls.Add(cmbRol);

            btnGuardar = new Button();
            btnGuardar.Text = "Actualizar";
            btnGuardar.Font = new Font("Arial", 9, FontStyle.Bold);
            btnGuardar.BackColor = Color.FromArgb(0, 122, 204);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(30, 235);
            btnGuardar.Size = new Size(140, 35);
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Click += BtnGuardar_Click;
            this.Controls.Add(btnGuardar);

            btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Font = new Font("Arial", 9, FontStyle.Bold);
            btnCancelar.BackColor = Color.FromArgb(108, 117, 125);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(190, 235);
            btnCancelar.Size = new Size(140, 35);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Click += (s, e) => this.Close();
            this.Controls.Add(btnCancelar);
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string contra = txtContra.Text.Trim();
            string rol = cmbRol.SelectedItem.ToString();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contra))
            {
                MessageBox.Show("Todos los campos son obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ConexionBD conexionBD = new ConexionBD();
            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            {
                try
                {
                    conexion.Open();
                    string query = "UPDATE Usuarios SET NombreUs = @NombreUs, contra = @contra, Rol = @Rol WHERE idUsuario = @idUsuario";
                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@NombreUs", usuario);
                        cmd.Parameters.AddWithValue("@contra", contra);
                        cmd.Parameters.AddWithValue("@Rol", rol);
                        cmd.Parameters.AddWithValue("@idUsuario", _idUsuario);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Usuario actualizado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _padre.CargarUsuarios();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al actualizar usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}