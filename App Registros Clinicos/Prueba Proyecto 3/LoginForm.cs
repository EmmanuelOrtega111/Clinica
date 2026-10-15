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
    public partial class LoginForm : Form
    {
        private Label lblTitulo;
        private Label lblUsuario;
        private TextBox txtUsuario;
        private Label lblRol;
        private ComboBox cmbRol;
        private Button btnIngresar;

        public LoginForm()
        {
            InitializeComponent();
            ConfigurarVentana();
        }

        private void ConfigurarVentana()
        {
            this.Text = "Unidad de Salud";
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
            lblUsuario.Location = new Point(40, 70);
            lblUsuario.Size = new Size(100, 20);
            this.Controls.Add(lblUsuario);

            txtUsuario = new TextBox();
            txtUsuario.Location = new Point(40, 95);
            txtUsuario.Size = new Size(300, 25);
            this.Controls.Add(txtUsuario);

            // Campo de Rol
            lblRol = new Label();
            lblRol.Text = "Rol del Sistema:";
            lblRol.Location = new Point(40, 135);
            lblRol.Size = new Size(100, 20);
            this.Controls.Add(lblRol);

            cmbRol = new ComboBox();
            cmbRol.Location = new Point(40, 160);
            cmbRol.Size = new Size(300, 25);
            cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRol.Items.AddRange(new object[] { "Administrador", "Médico" });
            cmbRol.SelectedIndex = 0;
            this.Controls.Add(cmbRol);

            // Botón de Ingresar
            btnIngresar = new Button();
            btnIngresar.Text = "Iniciar Sesión";
            btnIngresar.Location = new Point(40, 210);
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
            string rol = cmbRol.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(usuario))
            {
                MessageBox.Show("Por favor, ingrese un nombre de usuario.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SesionUsuario.Instancia.IniciarSesion(usuario, rol);

            MessageBox.Show($"Bienvenido, {usuario} [{rol}]", "Sesión Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Hide();
            MainForm principal = new MainForm();
            principal.ShowDialog();
            this.Close();
        }
    }
}
