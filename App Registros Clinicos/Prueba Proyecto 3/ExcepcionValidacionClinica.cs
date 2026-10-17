using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_Proyecto_3
{
    public class ExcepcionValidacionClinica : Exception
    {
        public ExcepcionValidacionClinica() : base("Error de validación en los registros clínicos del paciente.") { }
        public ExcepcionValidacionClinica(string mensaje) : base(mensaje) { }
        public ExcepcionValidacionClinica(string mensaje, Exception innerException) : base(mensaje, innerException) { }
    }
}