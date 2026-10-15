using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_Proyecto_3
{
    public interface IRepositorioPaciente
    {
        void RegistrarPaciente(Paciente nuevoPaciente);
        void MostrarTodos();
        Paciente BuscarPorExpediente(string numExpediente);
        void LimpiarLista();
    }
}
