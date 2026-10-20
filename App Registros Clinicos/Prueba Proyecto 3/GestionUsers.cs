using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using SistemaUnidadSalud.Data;

namespace Prueba_Proyecto_3
{
    public partial class GestionUsers : Form
    {
        private Label lblTitulo;
        private DataGridView dgvUsuarios;
        private Button btnAgregar;
        private Button btnEditar;
        private Button btnDesactivar;
        private Button btnVolver;

        public GestionUsers()
        {
            InitializeComponent();
            ConfigurarVentana();
            CargarUsuarios();
        }

        private void ConfigurarVentana()
        {
            this.Text = "Gestión de Usuarios del Sistema";
            this.Size = new Size(700, 480);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Título
            lblTitulo = new Label();
            lblTitulo.Text = "ADMINISTRACIÓN DE USUARIOS";
            lblTitulo.Font = new Font("Arial", 14, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(0, 102, 204);
            lblTitulo.Location = new Point(30, 20);
            lblTitulo.Size = new Size(400, 30);
            this.Controls.Add(lblTitulo);

            // DataGridView de Usuarios
            dgvUsuarios = new DataGridView
            {
                Location = new Point(30, 60),
                Size = new Size(620, 280),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false
            };
            this.Controls.Add(dgvUsuarios);

            // Botón Agregar
            btnAgregar = new Button();
            btnAgregar.Text = "Agregar Usuario";
            btnAgregar.Font = new Font("Arial", 9, FontStyle.Bold);
            btnAgregar.BackColor = Color.FromArgb(40, 167, 69);
            btnAgregar.ForeColor = Color.White;
            btnAgregar.Location = new Point(30, 360);
            btnAgregar.Size = new Size(140, 40);
            btnAgregar.FlatStyle = FlatStyle.Flat;
            btnAgregar.FlatAppearance.BorderSize = 0;
            btnAgregar.Click += BtnAgregar_Click;
            this.Controls.Add(btnAgregar);

            // Botón Editar
            btnEditar = new Button();
            btnEditar.Text = "Editar Usuario";
            btnEditar.Font = new Font("Arial", 9, FontStyle.Bold);
            btnEditar.BackColor = Color.FromArgb(0, 122, 204);
            btnEditar.ForeColor = Color.White;
            btnEditar.Location = new Point(185, 360);
            btnEditar.Size = new Size(140, 40);
            btnEditar.FlatStyle = FlatStyle.Flat;
            btnEditar.FlatAppearance.BorderSize = 0;
            btnEditar.Click += BtnEditar_Click;
            this.Controls.Add(btnEditar);

            // Botón Desactivar / Activar
            btnDesactivar = new Button();
            btnDesactivar.Text = "Cambiar Estado";
            btnDesactivar.Font = new Font("Arial", 9, FontStyle.Bold);
            btnDesactivar.BackColor = Color.FromArgb(255, 193, 7);
            btnDesactivar.ForeColor = Color.Black;
            btnDesactivar.Location = new Point(340, 360);
            btnDesactivar.Size = new Size(150, 40);
            btnDesactivar.FlatStyle = FlatStyle.Flat;
            btnDesactivar.FlatAppearance.BorderSize = 0;
            btnDesactivar.Click += BtnDesactivar_Click;
            this.Controls.Add(btnDesactivar);

            // Botón Volver
            btnVolver = new Button();
            btnVolver.Text = "Volver";
            btnVolver.Font = new Font("Arial", 9, FontStyle.Bold);
            btnVolver.BackColor = Color.FromArgb(108, 117, 125);
            btnVolver.ForeColor = Color.White;
            btnVolver.Location = new Point(510, 360);
            btnVolver.Size = new Size(140, 40);
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.FlatAppearance.BorderSize = 0;
            btnVolver.Click += (s, e) => this.Close();
            this.Controls.Add(btnVolver);
        }

        public void CargarUsuarios()
        {
            ConexionBD conexionBD = new ConexionBD();
            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            {
                try
                {
                    string query = @"
                        SELECT 
                            idUsuario AS [ID],
                            NombreUs AS [Usuario],
                            contra AS [Contraseña],
                            Rol AS [Rol],
                            CASE WHEN Activo = 1 THEN 'Activo' ELSE 'Inactivo' END AS [Estado]
                        FROM Usuarios";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conexion);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvUsuarios.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar usuarios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            agregarUsers frm = new agregarUsers(this);
            frm.ShowDialog();
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un usuario de la lista para editar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idUsuario = Convert.ToInt32(dgvUsuarios.SelectedRows[0].Cells["ID"].Value);
            string usuario = dgvUsuarios.SelectedRows[0].Cells["Usuario"].Value.ToString();
            string contra = dgvUsuarios.SelectedRows[0].Cells["Contraseña"].Value.ToString();
            string rol = dgvUsuarios.SelectedRows[0].Cells["Rol"].Value.ToString();

            editUsers frm = new editUsers(this, idUsuario, usuario, contra, rol);
            frm.ShowDialog();
        }

        private void BtnDesactivar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un usuario de la lista para cambiar su estado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idUsuario = Convert.ToInt32(dgvUsuarios.SelectedRows[0].Cells["ID"].Value);
            string usuario = dgvUsuarios.SelectedRows[0].Cells["Usuario"].Value.ToString();
            string estadoActual = dgvUsuarios.SelectedRows[0].Cells["Estado"].Value.ToString();

            bool nuevoEstado = estadoActual != "Activo";
            string accionText = nuevoEstado ? "activar" : "desactivar";

            DialogResult confirmacion = MessageBox.Show(
                $"¿Está seguro de que desea {accionText} al usuario '{usuario}'?",
                "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmacion == DialogResult.Yes)
            {
                ConexionBD conexionBD = new ConexionBD();
                using (SqlConnection conexion = conexionBD.ObtenerConexion())
                {
                    try
                    {
                        conexion.Open();
                        string query = "UPDATE Usuarios SET Activo = @Activo WHERE idUsuario = @idUsuario";
                        using (SqlCommand cmd = new SqlCommand(query, conexion))
                        {
                            cmd.Parameters.AddWithValue("@Activo", nuevoEstado ? 1 : 0);
                            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show($"Usuario '{usuario}' actualizado a {(nuevoEstado ? "Activo" : "Inactivo")}.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarUsuarios();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al cambiar el estado del usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}