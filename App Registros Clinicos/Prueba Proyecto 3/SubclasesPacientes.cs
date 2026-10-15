using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_Proyecto_3
{
   
    public class SolicitudLaboratorioEmbarazada
    {

        [Key]
        public int Id { get; set; }
        public string Fecha { get; set; }
        public string NombreExamen { get; set; }
        public string TipoMuestra { get; set; }
    }
 
    public class ConsumoYRiesgoTrimestre
    {
        public bool FumaActivamente { get; set; }
        public bool FumaPasivamente { get; set; }
        public bool Drogas { get; set; }
        public bool Alcohol { get; set; }
        public bool Violencia { get; set; }
    }

    public class RegistroVacuna
    {

        [Key]
        public int Id { get; set; }
        public string NombreVacuna { get; set; }
        public bool Aplicada { get; set; }
        public string FechaAplicacion { get; set; }
    }

    public class PerfilPrenatal
    {
        public string CitologiaFechaToma { get; set; }
        public string CitologiaFechaResultado { get; set; }
        public string CitologiaResultado { get; set; }
        public string GrupoSanguineo { get; set; }
        public string HemoglobinaMenor20Sem { get; set; }
        public string HemoglobinaMayor20Sem { get; set; }
        public string Chagas { get; set; }
        public string BacteriuriaMenor20Sem { get; set; }
        public string BacteriuriaMayor20Sem { get; set; }
        public string GlucemiaAyunoMenor30Sem { get; set; }
        public string EstreptococosB_30_37Sem { get; set; }
        public string VIH { get; set; }
        public string Sifilis { get; set; }
        public string UltrasonografiaObs { get; set; }
    }

    // ==========================================
    // PACIENTE EMBARAZADA
    // ==========================================

    public class PacienteEmbarazada : Paciente
    {
       
        public string FormulaObstetrica { get; set; }

        public string FinEmbarazoAnteriorFecha { get; set; }
        public string FinEmbarazoAnteriorTerminacion { get; set; }
        public bool EmbarazoNoDeseado { get; set; }
        public bool UsabaMetodoAnticonceptivo { get; set; }
        public string MetodoAnticonceptivoEmpleado { get; set; }

        public double PesoAnteriorKg { get; set; }
        public string EdadGestacionalTexto { get; set; }
        public DateTime FUR { get; set; }
        public DateTime FPP { get; set; }

        public ConsumoYRiesgoTrimestre Trimestre1 { get; set; } = new ConsumoYRiesgoTrimestre();
        public ConsumoYRiesgoTrimestre Trimestre2 { get; set; } = new ConsumoYRiesgoTrimestre();
        public ConsumoYRiesgoTrimestre Trimestre3 { get; set; } = new ConsumoYRiesgoTrimestre();
        public List<RegistroVacuna> Vacunas { get; set; } = new List<RegistroVacuna>();
        public PerfilPrenatal Prenatal { get; set; } = new PerfilPrenatal();
        public List<SolicitudLaboratorioEmbarazada> LaboratoriosEmbarazada { get; set; } = new List<SolicitudLaboratorioEmbarazada>();
        public PacienteEmbarazada() : base()
        {
            // Constructor vacío requerido por Entity Framework
        }

        public PacienteEmbarazada(string nombre, int edad, string expediente, string direccion, string responsable)
            : base(nombre, edad, expediente, "Femenino", direccion, responsable) { }

        public void CalcularFPP()
        {
            FPP = FUR.AddDays(7).AddMonths(-3).AddYears(1);
        }

        public override void MostrarHistorialCompleto()
        {
            base.MostrarHistorialCompleto();

            Console.WriteLine("\n--- DATOS OBSTÉTRICOS Y GESTACIÓN ACTUAL ---");
            Console.WriteLine($"Fórmula Obstétrica: {FormulaObstetrica}");
            Console.WriteLine($"Fin Embarazo Anterior: {FinEmbarazoAnteriorFecha} ({FinEmbarazoAnteriorTerminacion})");
            Console.WriteLine($"Embarazo no deseado: {(EmbarazoNoDeseado ? "Sí" : "No")} | Usó anticonceptivo: {(UsabaMetodoAnticonceptivo ? "Sí (" + MetodoAnticonceptivoEmpleado + ")" : "No")}");
            Console.WriteLine($"Peso Inicial: {PesoAnteriorKg} kg | Edad Gestacional: {EdadGestacionalTexto}");
            Console.WriteLine($"FUR: {FUR.ToShortDateString()} | FPP Estimada: {FPP.ToShortDateString()}");

            Console.WriteLine("\n--- USO DE ALCOHOL Y DROGAS POR TRIMESTRE ---");
            MostrarTrimestre("Primer Trimestre", Trimestre1);
            MostrarTrimestre("Segundo Trimestre", Trimestre2);
            MostrarTrimestre("Tercer Trimestre", Trimestre3);

            Console.WriteLine("\n--- VACUNACIÓN ---");
            foreach (var v in Vacunas)
            {
                string estado = v.Aplicada ? $"Aplicada el {v.FechaAplicacion}" : "No aplicada";
                Console.WriteLine($" - {v.NombreVacuna}: {estado}");
            }

            Console.WriteLine("\n--- PERFILES PRENATALES ---");
            Console.WriteLine($"Citología - Toma: {Prenatal.CitologiaFechaToma}, Res: {Prenatal.CitologiaFechaResultado}, Resultado: {Prenatal.CitologiaResultado}");
            Console.WriteLine($"Grupo Sanguíneo: {Prenatal.GrupoSanguineo}");
            Console.WriteLine($"Hemoglobina (<20 sem): {Prenatal.HemoglobinaMenor20Sem} | (>=20 sem): {Prenatal.HemoglobinaMayor20Sem}");
            Console.WriteLine($"Bacteriuria (<20 sem): {Prenatal.BacteriuriaMenor20Sem} | (>=20 sem): {Prenatal.BacteriuriaMayor20Sem}");
            Console.WriteLine($"Glucemia Ayuno (<30 sem): {Prenatal.GlucemiaAyunoMenor30Sem}");
            Console.WriteLine($"Estreptococos B (30-37 sem): {Prenatal.EstreptococosB_30_37Sem}");
            Console.WriteLine($"VIH: {Prenatal.VIH} | Sífilis: {Prenatal.Sifilis}");
            Console.WriteLine($"Ultrasonografía: {Prenatal.UltrasonografiaObs}");

            Console.WriteLine("\n--- SOLICITUD DE LABORATORIOS ---");
            foreach (var lab in LaboratoriosEmbarazada)
            {
                Console.WriteLine($" - [{lab.Fecha}] {lab.NombreExamen} (Muestra: {lab.TipoMuestra})");
            }
        }

        private void MostrarTrimestre(string nombre, ConsumoYRiesgoTrimestre t)
        {
            Console.WriteLine($" {nombre} -> Fuma Act: {(t.FumaActivamente ? "Sí" : "No")} | Fuma Pas: {(t.FumaPasivamente ? "Sí" : "No")} | Drogas: {(t.Drogas ? "Sí" : "No")} | Alcohol: {(t.Alcohol ? "Sí" : "No")} | Violencia: {(t.Violencia ? "Sí" : "No")}");
        }
    }
    // ==========================================
    // PACIENTE INFANTIL
    // ==========================================
    public class PacienteInfantil : Paciente
    {
        // Evaluación Micronutrientes (Sí/No)
        public bool VitaminaA { get; set; }
        public bool Multivitaminas { get; set; }
        public bool Hierro { get; set; }
        public bool Mebendazol { get; set; }
        public bool Albendazol { get; set; }

        // Vacunación, Violencia y Derechos (Sí/No)
        public bool EsquemaVacunacionCompleto { get; set; }
        public bool SufreMaltratoOViolencia { get; set; }
        public bool CumpleDerechosNino { get; set; }
        public bool RiesgoSocial { get; set; }
        public PacienteInfantil() : base()
        {
            // Constructor vacío requerido por Entity Framework
        }
        public PacienteInfantil(string nom, int ed, string exp, string sx, string dir, string resp)
            : base(nom, ed, exp, sx, dir, resp) { }

        public override void MostrarHistorialCompleto()
        {
            base.MostrarHistorialCompleto();
            Console.WriteLine("\n--- EVALUACIÓN DE MICRONUTRIENTES ---");
            Console.WriteLine($"Vitamina A: {(VitaminaA ? "Sí" : "No")} | Multivitaminas: {(Multivitaminas ? "Sí" : "No")} | Hierro: {(Hierro ? "Sí" : "No")}");
            Console.WriteLine($"Mebendazol: {(Mebendazol ? "Sí" : "No")} | Albendazol: {(Albendazol ? "Sí" : "No")}");

            Console.WriteLine("\n--- VACUNACIÓN Y PROTECCIÓN SOCIAL ---");
            Console.WriteLine($"Esquema Vacunación Completo: {(EsquemaVacunacionCompleto ? "Sí" : "No")}");
            Console.WriteLine($"Sufre Maltrato/Violencia: {(SufreMaltratoOViolencia ? "Sí" : "No")}");
            Console.WriteLine($"Cumple Derechos Niño/a: {(CumpleDerechosNino ? "Sí" : "No")}");
            Console.WriteLine($"Riesgo Social: {(RiesgoSocial ? "Sí" : "No")}");
        }
    }

    // ==========================================
    //  PACIENTE ADOLESCENTE
    // ==========================================
    public class PacienteAdolescente : Paciente
    {
        public PacienteAdolescente() : base()
        {
            // Constructor vacío requerido por Entity Framework
        }

        public Dictionary<string, bool> AntecedentesFamiliares { get; set; } = new Dictionary<string, bool>();

        
        public List<AntecedentePersonal> AntecedentesPersonales { get; set; } = new List<AntecedentePersonal>();

     
        public bool Etilista { get; set; }
        public bool Tabaquista { get; set; }
        public bool Drogadiccion { get; set; }

     
        public string EscalaTanner { get; set; }
        public string PanelSnellen { get; set; }

       
        public List<string> LaboratoriosSolicitados { get; set; } = new List<string>();

        public PacienteAdolescente(string nom, int ed, string exp, string sx, string dir, string resp)
            : base(nom, ed, exp, sx, dir, resp) { }

        public override void MostrarHistorialCompleto()
        {
            base.MostrarHistorialCompleto();

            Console.WriteLine("\n--- ANTECEDENTES FAMILIARES PATOLÓGICOS ---");
            foreach (var ant in AntecedentesFamiliares)
            {
                Console.WriteLine($" - {ant.Key}: {(ant.Value ? "Sí" : "No")}");
            }

            Console.WriteLine("\n--- ANTECEDENTES PERSONALES PATOLÓGICOS ---");
            if (AntecedentesPersonales.Count == 0) Console.WriteLine(" Sin antecedentes patológicos personales registrados.");
            else
            {
                foreach (var ap in AntecedentesPersonales)
                {
                    Console.WriteLine($" - {ap.Enfermedad} (Inicio: {ap.AnioInicio}, Med: {ap.TipoMedication})");
                }
            }

            Console.WriteLine("\n--- CONSUMO DE SUSTANCIAS ---");
            Console.WriteLine($"Etilista: {(Etilista ? "Sí" : "No")} | Tabaquista: {(Tabaquista ? "Sí" : "No")} | Drogadicción: {(Drogadiccion ? "Sí" : "No")}");

            Console.WriteLine("\n--- EVALUACIONES Y LABORATORIOS ---");
            Console.WriteLine($"Escala de Tanner: {EscalaTanner}");
            Console.WriteLine($"Panel de Snellen: {PanelSnellen}");
            Console.WriteLine("Exámenes de laboratorio solicitados:");
            foreach (var lab in LaboratoriosSolicitados) Console.WriteLine($" - {lab}");
        }
    }

    // ==========================================
    // PACIENTE ADULTO MAYOR
    // ==========================================
    public class PacienteAdultoMayor : Paciente
    {
        // Puntaje numérico y categoría calculada
        public int PuntajeBarthel { get; set; }
        public string CategoriaDependencia { get; set; }

        public PacienteAdultoMayor() : base()
        {
            // Constructor vacío requerido por Entity Framework
        }
        public PacienteAdultoMayor(string nom, int ed, string exp, string sx, string dir, string resp)
            : base(nom, ed, exp, sx, dir, resp) { }

        public override void MostrarHistorialCompleto()
        {
            base.MostrarHistorialCompleto();
            Console.WriteLine("\n--- EVALUACIÓN GERIÁTRICA (ÍNDICE DE BARTHEL) ---");
            Console.WriteLine($"Puntuación Total: {PuntajeBarthel} / 100 pts");
            Console.WriteLine($"Grado de Dependencia: {CategoriaDependencia}");
        }
    }

    // ==========================================
    //  PACIENTE CRÓNICO
    // ==========================================
    public class PacienteCronico : Paciente
    {
        public PacienteCronico() : base()
        {
            // Constructor vacío requerido por Entity Framework
        }

        public PacienteCronico(string nom, int ed, string exp, string sx, string dir, string resp)
            : base(nom, ed, exp, sx, dir, resp) { }
    }

    // ==========================================
    //  PACIENTE MUJER EN EDAD FÉRTIL
    // ==========================================
    public class PacienteMujerEdadFertil : Paciente
    {
        public string MetodoAnticonceptivo { get; set; } 
        public string FechaUltimaMenstruacion { get; set; }
        public string FechaUltimoParto { get; set; }
        public string FormulaObstetrica { get; set; }
        public PacienteMujerEdadFertil() : base()
        {
            // Constructor vacío requerido por Entity Framework
        }
        public PacienteMujerEdadFertil(string nom, int ed, string exp, string dir, string resp)
            : base(nom, ed, exp, "Femenino", dir, resp) { }

        public override void MostrarHistorialCompleto()
        {
            base.MostrarHistorialCompleto();
            Console.WriteLine("\n--- DATOS SALUD REPRODUCTIVA Y OBSTÉTRICA PREVIA ---");
            Console.WriteLine($"Método Anticonceptivo: {MetodoAnticonceptivo}");
            Console.WriteLine($"Fecha Última Menstruación (FUM): {FechaUltimaMenstruacion}");
            Console.WriteLine($"Fecha Último Parto (FUP): {FechaUltimoParto}");
            Console.WriteLine($"Fórmula Obstétrica: {FormulaObstetrica}");
        }
    }
}
