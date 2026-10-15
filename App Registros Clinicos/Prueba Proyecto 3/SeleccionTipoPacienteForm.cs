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
    public partial class SeleccionTipoPacienteForm : Form
    {
        private ComboBox cmbTipos;
        private Button btnContinuar;
        private Label lblInstruccion;

        private void SeleccionTipoPacienteForm_Load(object sender, EventArgs e)
        {
            // Lógica inicial del formulario al cargar, si se requiere
        }



        public SeleccionTipoPacienteForm()
        {
            InitializeComponent();
            ConfigurarVentana();
        }

        private void ConfigurarVentana()
        {
            this.Text = "Seleccionar Perfil Clínico";
            this.Size = new Size(380, 220);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            lblInstruccion = new Label();
            lblInstruccion.Text = "Seleccione el tipo de paciente que desea registrar:";
            lblInstruccion.Font = new Font("Arial", 9.5f, FontStyle.Bold);
            lblInstruccion.Location = new Point(30, 25);
            lblInstruccion.Size = new Size(310, 25);
            this.Controls.Add(lblInstruccion);

            cmbTipos = new ComboBox();
            cmbTipos.Location = new Point(30, 60);
            cmbTipos.Size = new Size(300, 25);
            cmbTipos.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipos.Items.AddRange(new object[] {
                "Paciente Crónico",
                "Paciente Infantil",
                "Paciente Adolescente",
                "Paciente Adulto Mayor",
                "Paciente Mujer en Edad Fértil",
                "Paciente Embarazada"
            });
            cmbTipos.SelectedIndex = 0;
            this.Controls.Add(cmbTipos);

            btnContinuar = new Button();
            btnContinuar.Text = "Continuar al Registro";
            btnContinuar.Location = new Point(30, 110);
            btnContinuar.Size = new Size(300, 35);
            btnContinuar.BackColor = Color.FromArgb(0, 122, 204);
            btnContinuar.ForeColor = Color.White;
            btnContinuar.Font = new Font("Arial", 9, FontStyle.Bold);
            btnContinuar.Click += BtnContinuar_Click;
            this.Controls.Add(btnContinuar);
        }

        private void BtnContinuar_Click(object sender, EventArgs e)
        {
            string seleccionado = cmbTipos.SelectedItem.ToString();
            this.Hide();

            switch (seleccionado)
            {
                case "Paciente Crónico":
                    new RegistroCronicoForm().ShowDialog();
                    break;
                case "Paciente Infantil":
                    new RegistroInfantilForm().ShowDialog();
                    break;
                case "Paciente Adolescente":
                    new RegistroAdolescenteForm().ShowDialog();
                    break;
                case "Paciente Adulto Mayor":
                    new RegistroAdultoMayorForm().ShowDialog();
                    break;
                case "Paciente Mujer en Edad Fértil":
                    new RegistroMujerEdadFertilForm().ShowDialog();
                    break;
                case "Paciente Embarazada":
                    new RegistroEmbarazadaForm().ShowDialog();
                    break;
            }

            this.Close();
        }
    }
}
