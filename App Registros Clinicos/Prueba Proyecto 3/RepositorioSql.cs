using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_Proyecto_3
{
    public class RepositorioSql : IRepositorioPaciente
    {
        public void RegistrarPaciente(Paciente nuevoPaciente)
        {
            using (var context = new AppDbContext())
            {
                context.Pacientes.Add(nuevoPaciente);
                context.SaveChanges();

                Console.WriteLine($"\n[ÉXITO BD]: Paciente {nuevoPaciente.Nombre} guardado en la Base de Datos con Expediente N° {nuevoPaciente.NumeroExpediente}.");
            }
        }

        public void MostrarTodos()
        {
            using (var context = new AppDbContext())
            {
                var lista = context.Pacientes.ToList();
                if (lista.Count == 0)
                {
                    Console.WriteLine("\nNo hay expedientes registrados en la base de datos.");
                    return;
                }

                foreach (var p in lista)
                {
                    p.MostrarHistorialCompleto();
                }
            }
        }

        public Paciente BuscarPorExpediente(string numExpediente)
        {
            using (var context = new AppDbContext())
            {
                return context.Pacientes
                    .FirstOrDefault(p => p.NumeroExpediente.ToLower() == numExpediente.ToLower());
            }
        }

        public void LimpiarLista()
        {
            using (var context = new AppDbContext())
            {
                context.Pacientes.RemoveRange(context.Pacientes);
                context.SaveChanges();
            }
        }
    }
}
