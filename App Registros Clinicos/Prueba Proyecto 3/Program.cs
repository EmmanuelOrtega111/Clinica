using System;
using System.Windows.Forms;

namespace Prueba_Proyecto_3
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            while (true)
            {
                // Reiniciamos la bandera de sesión cada vez que mostramos el Login
                LoginForm.SesionCerradaParaReinicio = false;

                using (LoginForm login = new LoginForm())
                {
                    if (login.ShowDialog() == DialogResult.OK)
                    {
                        using (MainForm principal = new MainForm())
                        {
                            principal.ShowDialog();
                        }

                        // Si al cerrar MainForm NO fue por darle a "Cerrar Sesión", salimos del bucle
                        if (!LoginForm.SesionCerradaParaReinicio)
                        {
                            break;
                        }
                    }
                    else
                    {
                        // Si se cerró la ventana de Login con la 'X' o se canceló, sale de la app
                        break;
                    }
                }
            }
        }
    }
}