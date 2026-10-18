using System;
using System.Linq;

namespace Prueba_Proyecto_3
{
    public class RepositorioSql : IRepositorioPaciente
    {
        public void RegistrarPaciente(Paciente paciente, Consulta consulta)
        {
            using (var context = new AppDbContext())
            {
                // 1. Verificar si el paciente ya existe; si no, agregarlo
                var pacienteExistente = context.Pacientes.Find(paciente.NumeroExpediente);
                if (pacienteExistente == null)
                {
                    context.Pacientes.Add(paciente);
                }

                // 2. Asociar la consulta al paciente
                consulta.NumeroExpediente = paciente.NumeroExpediente;
                context.Consultas.Add(consulta);

                // 3. Guardar cambios en cascada
                context.SaveChanges();
            }
        }

        public void RegistrarPaciente(Paciente nuevoPaciente)
        {
            using (var context = new AppDbContext())
            {
                var existe = context.Pacientes.Find(nuevoPaciente.NumeroExpediente);
                if (existe == null)
                {
                    context.Pacientes.Add(nuevoPaciente);
                    context.SaveChanges();
                }
            }
        }

        public void MostrarTodos()
        {
            using (var context = new AppDbContext())
            {
                var lista = context.Pacientes.Include("Consultas").ToList();
                foreach (var p in lista)
                {
                    Console.WriteLine($"Expediente: {p.NumeroExpediente} - Nombre: {p.Nombre} - Consultas: {p.Consultas.Count}");
                }
            }
        }

        public Paciente BuscarPorExpediente(string numExpediente)
        {
            using (var context = new AppDbContext())
            {
                return context.Pacientes
                    .Include("Consultas")
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