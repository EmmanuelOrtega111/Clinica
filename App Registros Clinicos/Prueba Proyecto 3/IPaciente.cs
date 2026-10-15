using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_Proyecto_3
{
    public interface IPaciente
    {
        string NumeroExpediente { get; set; }
        string Nombre { get; set; }
        int Edad { get; set; }
        string Sexo { get; set; }
        DateTime FechaConsulta { get; set; }

        void MostrarHistorialCompleto();
    }
}
