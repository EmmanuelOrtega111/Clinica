using System.Data.Entity;

namespace Prueba_Proyecto_3
{
    public class AppDbContext : DbContext
    {
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<ExamenFisico> ExamenesFisicos { get; set; }
        public DbSet<Medicamento> Recetas { get; set; }
        public DbSet<AntecedentePersonal> AntecedentesPersonales { get; set; }
        public DbSet<DetalleEmbarazada> DetallesEmbarazada { get; set; }
        public DbSet<DetalleAdultoMayor> DetallesAdultoMayor { get; set; }

        public AppDbContext() : base("name=ConexionClinica")
        {
            Database.SetInitializer<AppDbContext>(null); // Desactiva la recreación automática para alinearse al script SQL
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapeo Clave Primaria String en Paciente
            modelBuilder.Entity<Paciente>()
                .HasKey(p => p.NumeroExpediente);

            // Configuración 1 a Muchos: Paciente -> Consultas
            modelBuilder.Entity<Consulta>()
                .HasRequired(c => c.Paciente)
                .WithMany(p => p.Consultas)
                .HasForeignKey(c => c.NumeroExpediente)
                .WillCascadeOnDelete(true);

            // Configuración 1 a Muchos: Consulta -> ExamenesFisicos
            modelBuilder.Entity<ExamenFisico>()
                .HasRequired(e => e.Consulta)
                .WithMany(c => c.ExamenesFisicos)
                .HasForeignKey(e => e.IdConsulta)
                .WillCascadeOnDelete(true);

            // Configuración 1 a Muchos: Consulta -> Recetas
            modelBuilder.Entity<Medicamento>()
                .HasRequired(m => m.Consulta)
                .WithMany(c => c.Recetas)
                .HasForeignKey(m => m.IdConsulta)
                .WillCascadeOnDelete(true);

            // Configuración 1 a 1: Consulta -> DetalleEmbarazada
            modelBuilder.Entity<Consulta>()
                .HasOptional(c => c.DetalleEmbarazada)
                .WithRequired(d => d.Consulta)
                .WillCascadeOnDelete(true);

            // Configuración 1 a 1: Consulta -> DetalleAdultoMayor
            modelBuilder.Entity<Consulta>()
                .HasOptional(c => c.DetalleAdultoMayor)
                .WithRequired(d => d.Consulta)
                .WillCascadeOnDelete(true);
        }
    }
}