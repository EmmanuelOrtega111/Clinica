using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Prueba_Proyecto_3
{

    // Estructura para registrar si cada área examinada es normal o anormal
    public class HallazgoExamen
    {
        public bool EsNormal { get; set; } = true;
        public string Detalle { get; set; } = "Sin hallazgos";
    }

    // Estructura para medicamentos recetados
    public class Medicamento
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string CantidadYDosis { get; set; }

        public Medicamento(string nombre, string cantidadYDosis)
        {
            Nombre = nombre;
            CantidadYDosis = cantidadYDosis;
        }
    }

    // Estructura para antecedentes personales patológicos. 
    public class AntecedentePersonal
    {

        [Key]
        public int Id { get; set; }
        public string Enfermedad { get; set; }
        public int AnioInicio { get; set; }
        public string TipoMedication { get; set; }
    }

    // Clase base abstracta Paciente
    public abstract class Paciente: IPaciente
    {
        [Key]
        public int Id { get; set; }

        // Datos generales
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public string NumeroExpediente { get; set; }
        public string Sexo { get; set; }
        public string Direccion { get; set; }
        public string Responsable { get; set; }
        public DateTime FechaConsulta { get; set; } = DateTime.Now;

        // Motivo de Consulta
        public string ConsultaPor { get; set; }
        public string PresenteEnfermedad { get; set; }

        // Signos Vitales y Medidas Antropométricas
        public double TemperaturaCelsius { get; set; }
        public int FrecuenciaRespiratoria { get; set; }
        public int FrecuenciaCardiaca { get; set; }
        public int Pulso { get; set; }
        public string PresionArterial { get; set; }
        public double SaturacionOxigeno { get; set; }
        public double PesoKg { get; set; }
        public double TallaCm { get; set; }
        public double CircunferenciaAbdominalCm { get; set; }

        // Cálculo de IMC
        public double IMC
        {
            get
            {
                if (TallaCm <= 0) return 0;
                double tallaMetros = TallaCm / 100.0;
                return Math.Round(PesoKg / (tallaMetros * tallaMetros), 2);
            }
        }

        // Examen Físico (Opciones entre Normal/Anormal)
        public Dictionary<string, HallazgoExamen> ExamenFisico { get; set; } = new Dictionary<string, HallazgoExamen>();

        // Diagnóstico y Tratamiento
        public string TipoDiagnostico { get; set; } // Principal o Secundario
        public string DiagnosticoTexto { get; set; }
        public string TipoConsulta { get; set; } // Primera vez o Subsecuente
        public List<Medicamento> Receta { get; set; } = new List<Medicamento>();
        public string Recomendaciones { get; set; }

        public Paciente()
        {

        }
        // Constructor base
        public Paciente(string nombre, int edad, string expediente, string sexo, string direccion, string responsable)
        {
            Nombre = nombre;
            Edad = edad;
            NumeroExpediente = expediente;
            Sexo = sexo;
            Direccion = direccion;
            Responsable = responsable;
        }
        

        public virtual void MostrarHistorialCompleto()
        {
            Console.WriteLine("\n=======================================================");
            Console.WriteLine($"      EXPEDIENTE MÉDICO: {NumeroExpediente}");
            Console.WriteLine("=======================================================");
            Console.WriteLine($"Fecha Consulta: {FechaConsulta.ToShortDateString()}");
            Console.WriteLine($"Paciente: {Nombre} | Edad: {Edad} años | Sexo: {Sexo}");
            Console.WriteLine($"Dirección: {Direccion} | Responsable/Tutor: {Responsable}");

            Console.WriteLine("\n--- MOTIVO DE CONSULTA ---");
            Console.WriteLine($"Consulta Por: {ConsultaPor}");
            Console.WriteLine($"Presente Enfermedad: {PresenteEnfermedad}");

            Console.WriteLine("\n--- SIGNOS VITALES Y MEDIDAS ---");
            Console.WriteLine($"Temp: {TemperaturaCelsius}°C | FC: {FrecuenciaCardiaca} lpm | Pulso: {Pulso} | FR: {FrecuenciaRespiratoria} rpm");
            Console.WriteLine($"PA: {PresionArterial} mmHg | SpO2: {SaturacionOxigeno}%");
            Console.WriteLine($"Peso: {PesoKg} kg | Talla: {TallaCm} cm | IMC: {IMC} | Cir. Abdominal: {CircunferenciaAbdominalCm} cm");

            Console.WriteLine("\n--- EXAMEN FÍSICO ---");
            foreach (var area in ExamenFisico)
            {
                string estado = area.Value.EsNormal ? "[NORMAL]" : "[ANORMAL]";
                Console.WriteLine($" - {area.Key.PadRight(22)}: {estado} -> {area.Value.Detalle}");
            }

            Console.WriteLine("\n--- DIAGNÓSTICO Y TRATAMIENTO ---");
            Console.WriteLine($"Tipo Diagnóstico: {TipoDiagnostico} | Consulta: {TipoConsulta}");
            Console.WriteLine($"Diagnóstico: {DiagnosticoTexto}");
            Console.WriteLine("Receta Médica:");
            if (Receta.Count == 0)
            {
                Console.WriteLine("  (No se prescribieron medicamentos)");
            }
            else
            {
                for (int i = 0; i < Receta.Count; i++)
                {
                    Console.WriteLine($"  {i + 1}. {Receta[i].Nombre} - Dosis/Cant: {Receta[i].CantidadYDosis}");
                }
            }
            Console.WriteLine($"Recomendaciones: {Recomendaciones}");
        }
    
        // Destructor
        ~Paciente()
        {
            Console.WriteLine($"\n[DESTRUCTOR]: El expediente N° {NumeroExpediente} de '{Nombre}' ha sido liberado de la memoria RAM.");
        }
    }
}
