using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_Proyecto_3
{
    public class SesionUsuario
    {
        private static SesionUsuario _instancia;
        private static readonly object _lock = new object();

        public string NombreUsuario { get; private set; }
        public string Rol { get; private set; }
        public bool EstaAutenticado => !string.IsNullOrEmpty(NombreUsuario);

        private SesionUsuario() { }

        public static SesionUsuario Instancia
        {
            get
            {
                lock (_lock)
                {
                    if (_instancia == null)
                    {
                        _instancia = new SesionUsuario();
                    }
                    return _instancia;
                }
            }
        }

        public void IniciarSesion(string usuario, string rol)
        {
            NombreUsuario = usuario;
            Rol = rol;
            Console.WriteLine($"\n[SESIÓN INICIADA]: Usuario '{NombreUsuario}' con rol [{Rol}].");
        }

        public void CerrarSesion()
        {
            Console.WriteLine($"\n[SESIÓN CERRADA]: Salió el usuario '{NombreUsuario}'.");
            NombreUsuario = null;
            Rol = null;
        }


    }
}
