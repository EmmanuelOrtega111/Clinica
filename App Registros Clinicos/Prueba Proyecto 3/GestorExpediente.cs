using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_Proyecto_3
{
    public class GestorExpedientes
    {
        private List<Paciente> listaPacientes = new List<Paciente>();

        public void RegistrarPaciente(Paciente nuevoPaciente)
        {
            listaPacientes.Add(nuevoPaciente);
            Console.WriteLine($"\n[ÉXITO]: Paciente {nuevoPaciente.Nombre} guardado con Expediente N° {nuevoPaciente.NumeroExpediente}.");
        }

        public void MostrarTodos()
        {
            if (listaPacientes.Count == 0)
            {
                Console.WriteLine("\nNo hay expedientes en la lista.");
                return;
            }

            foreach (var p in listaPacientes)
            {
                p.MostrarHistorialCompleto();
            }
        }

        public Paciente BuscarPorExpediente(string numExpediente)
        {
            return listaPacientes.Find(p => p.NumeroExpediente.Equals(numExpediente, StringComparison.OrdinalIgnoreCase));
        }

        public void LimpiarLista()
        {
            listaPacientes.Clear();
        }
    }
}
