using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_Proyecto_3
{
    public class GestorExpedientes : IRepositorioPaciente
    {

        // 1. Instancia estática privada única
        private static GestorExpedientes _instancia;
        private static readonly object _lock = new object();

        // 2. Lista privada de pacientes
        private List<Paciente> listaPacientes = new List<Paciente>();

        // 3. Constructor privado para evitar instanciación directa con 'new'
        private GestorExpedientes() { }

        // 4. Método público estático para obtener la única instancia 
        public static GestorExpedientes Instancia
        {
            get
            {
                lock (_lock)
                {
                    if (_instancia == null)
                    {
                        _instancia = new GestorExpedientes();
                    }
                    return _instancia;
                }
            }
        }

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
