using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_Proyecto_3
{
    internal class Program
    {
        private static void EvaluarIndiceBarthel(PacienteAdultoMayor p)
        {
            Console.WriteLine("\n--- EVALUACIÓN DE ÍNDICE DE BARTHEL (ACTIVIDADES DE LA VIDA DIARIA) ---");
            int suma = 0;

            Console.WriteLine("1. Comer:");
            Console.WriteLine("  0 = Incapaz | 5 = Necesita ayuda | 10 = Independiente");
            suma += CapturadorDatos.LeerEntero("  Seleccione opción (0, 5, 10)", 0, 10);

            Console.WriteLine("2. Lavarse / Bañarse:");
            Console.WriteLine("  0 = Dependiente | 5 = Independiente");
            suma += CapturadorDatos.LeerEntero("  Seleccione opción (0, 5)", 0, 5);

            Console.WriteLine("3. Vestirse:");
            Console.WriteLine("  0 = Dependiente | 5 = Necesita ayuda | 10 = Independiente");
            suma += CapturadorDatos.LeerEntero("  Seleccione opción (0, 5, 10)", 0, 10);

            Console.WriteLine("4. Arreglarse / Aseo personal:");
            Console.WriteLine("  0 = Dependiente | 5 = Independiente");
            suma += CapturadorDatos.LeerEntero("  Seleccione opción (0, 5)", 0, 5);

            Console.WriteLine("5. Deposición (Control anal):");
            Console.WriteLine("  0 = Incontinente | 5 = Accidente ocasional | 10 = Continente");
            suma += CapturadorDatos.LeerEntero("  Seleccione opción (0, 5, 10)", 0, 10);

            Console.WriteLine("6. Micción (Control vesical):");
            Console.WriteLine("  0 = Incontinente | 5 = Accidente ocasional | 10 = Continente");
            suma += CapturadorDatos.LeerEntero("  Seleccione opción (0, 5, 10)", 0, 10);

            Console.WriteLine("7. Uso del retrete:");
            Console.WriteLine("  0 = Dependiente | 5 = Necesita alguna ayuda | 10 = Independiente");
            suma += CapturadorDatos.LeerEntero("  Seleccione opción (0, 5, 10)", 0, 10);

            Console.WriteLine("8. Trasladarse (Sillón / Cama):");
            Console.WriteLine("  0 = Incapaz / No se mantiene sentado | 5 = Gran ayuda | 10 = Mínima ayuda | 15 = Independiente");
            suma += CapturadorDatos.LeerEntero("  Seleccione opción (0, 5, 10, 15)", 0, 15);

            Console.WriteLine("9. Deambulación (Caminar):");
            Console.WriteLine("  0 = Inmóvil | 5 = En silla de ruedas | 10 = Necesita ayuda de 1 persona | 15 = Independiente");
            suma += CapturadorDatos.LeerEntero("  Seleccione opción (0, 5, 10, 15)", 0, 15);

            Console.WriteLine("10. Escalones / Escaleras:");
            Console.WriteLine("  0 = Incapaz | 5 = Necesita ayuda | 10 = Independiente");
            suma += CapturadorDatos.LeerEntero("  Seleccione opción (0, 5, 10)", 0, 10);

            p.PuntajeBarthel = suma;

            // Determinación del diagnóstico funcional
            if (suma < 20) p.CategoriaDependencia = "Dependencia Total";
            else if (suma <= 35) p.CategoriaDependencia = "Dependencia Grave";
            else if (suma <= 55) p.CategoriaDependencia = "Dependencia Moderada";
            else if (suma <= 90) p.CategoriaDependencia = "Dependencia Leve";
            else p.CategoriaDependencia = "Independiente";

            Console.WriteLine($"\n [RESULTADO BARTHEL]: {p.PuntajeBarthel} pts -> {p.CategoriaDependencia}");
        }
        private static GestorExpedientes gestor = new GestorExpedientes();

        static void Main(string[] args)
        {
            bool salir = false;
            while (!salir)
            {
                Console.WriteLine("\n=======================================================");
                Console.WriteLine("        SISTEMA DE REGISTRO UNIDAD DE SALUD");
                Console.WriteLine("=======================================================");
                Console.WriteLine("1. Registrar Paciente Infantil");
                Console.WriteLine("2. Registrar Paciente Adolescente");
                Console.WriteLine("3. Registrar Paciente Adulto Mayor");
                Console.WriteLine("4. Registrar Paciente Crónico");
                Console.WriteLine("5. Registrar Paciente Mujer en Edad Fértil");
                Console.WriteLine("6. Registrar Paciente Embarazada");
                Console.WriteLine("7. Ver Todos los Expedientes Registrados");
                Console.WriteLine("8. Salir");
                Console.WriteLine("=======================================================");

                int op = CapturadorDatos.LeerEntero("Seleccione una opción", 1, 8);

                switch (op)
                {
                    case 1: CapturarInfantil(); break;
                    case 2: CapturarAdolescente(); break;
                    case 3: CapturarAdultoMayor(); break;
                    case 4: CapturarCronico(); break;
                    case 5: CapturarMujerEdadFertil(); break;
                    case 6: CapturarEmbarazada(); break;
                    case 7: gestor.MostrarTodos(); break;
                    case 8: salir = true; break;
                }
            }
        }

        // --- metodos de captura ---

        private static void CapturarMotivoYSignos(Paciente p)
        {
            Console.WriteLine("\n--- MOTIVO DE CONSULTA ---");
            p.ConsultaPor = CapturadorDatos.LeerTextoOBLIGATORIO("Consulta por");
            p.PresenteEnfermedad = CapturadorDatos.LeerTextoOBLIGATORIO("Presente Enfermedad");

            Console.WriteLine("\n--- SIGNOS VITALES Y MEDIDAS ANTROPOMÉTRICAS ---");
            p.TemperaturaCelsius = CapturadorDatos.LeerDecimal("Temperatura (°C, 30 - 45)", 30.0, 45.0);
            p.FrecuenciaRespiratoria = CapturadorDatos.LeerEntero("Frecuencia Respiratoria (rpm, 5 - 80)", 5, 80);
            p.FrecuenciaCardiaca = CapturadorDatos.LeerEntero("Frecuencia Cardíaca (lpm, 30 - 220)", 30, 220);
            p.Pulso = CapturadorDatos.LeerEntero("Pulso (ppm, 30 - 220)", 30, 220);
            p.PresionArterial = CapturadorDatos.LeerTextoOBLIGATORIO("Presión Arterial (mmHg, 120/80 - 180/120)");
            p.SaturacionOxigeno = CapturadorDatos.LeerDecimal("Saturación de Oxígeno (%, 50 - 100)", 50.0, 100.0);
            p.PesoKg = CapturadorDatos.LeerDecimal("Peso (kg, 1 - 300)", 1.0, 300.0);
            p.TallaCm = CapturadorDatos.LeerDecimal("Talla (cm, 20 - 250)", 20.0, 250.0);
            Console.WriteLine($"   [IMC Calculado]: {p.IMC} kg/m²");
            p.CircunferenciaAbdominalCm = CapturadorDatos.LeerDecimal("Circunferencia Abdominal (cm, 10 - 200)", 10.0, 200.0);
        }

        private static void CapturarExamenFisicoCompleto(Paciente p)
        {
            Console.WriteLine("\n=======================================================");
            Console.WriteLine("           EXAMEN FÍSICO");
            Console.WriteLine("=======================================================");

            string[] regiones = new string[]
            {
                "Apariencia general", "Cabeza", "Cuello",
                "Tórax y pulmonar", "Cardíaco", "Abdomen",
                "Columna Vertebral", "Región Glútea", "Genitales", 
                "Extremidades", "Piel y anexos", "Neurologico"
            };

            foreach (var reg in regiones)
            {
                bool esNormal = CapturadorDatos.LeerBooleano($"¿[{reg}] se encuentra NORMAL?");
                string detalle = "";

                if (esNormal)
                {
                    Console.Write($"Observaciones adicionales para [{reg}] (Enter para omitir): ");
                    detalle = Console.ReadLine()?.Trim();
                    if (string.IsNullOrWhiteSpace(detalle)) detalle = "Sin hallazgos / Normal";
                }
                else
                {
                    detalle = CapturadorDatos.LeerTextoOBLIGATORIO($"Describa la alteración/hallazgo anormal en [{reg}]");
                }

                p.ExamenFisico[reg] = new HallazgoExamen { EsNormal = esNormal, Detalle = detalle };
            }
        }

        private static void CapturarDiagnosticoYTratamiento(Paciente p)
        {
            Console.WriteLine("\n--- DIAGNÓSTICO ---");
            int tipoDiag = CapturadorDatos.LeerEntero("Tipo de Diagnóstico (1 = Principal / 2 = Secundario)", 1, 2);
            p.TipoDiagnostico = (tipoDiag == 1) ? "Principal" : "Secundario";
            p.DiagnosticoTexto = CapturadorDatos.LeerTextoOBLIGATORIO("Diagnóstico Médico");

            int tipoCons = CapturadorDatos.LeerEntero("Tipo de Consulta (1 = Primera vez / 2 = Subsecuente)", 1, 2);
            p.TipoConsulta = (tipoCons == 1) ? "Primera vez" : "Subsecuente";

            Console.WriteLine("\n--- TRATAMIENTO Y PRESCRIPCIÓN ---");
            int numMeds = CapturadorDatos.LeerEntero("¿Cuántos medicamentos prescribirá? (Máximo 5)", 0, 5);
            for (int i = 0; i < numMeds; i++)
            {
                string nombreMed = CapturadorDatos.LeerTextoOBLIGATORIO($"Nombre Medicamento #{i + 1}");
                string dosisMed = CapturadorDatos.LeerTextoOBLIGATORIO($"Cantidad y Dosis #{i + 1}");
                p.Receta.Add(new Medicamento(nombreMed, dosisMed));
            }

            Console.WriteLine("\n--- RECOMENDACIONES ---");
            p.Recomendaciones = CapturadorDatos.LeerTextoOBLIGATORIO("Recomendaciones Médicas");
        }

        // --- SUBCLASE INFANTIL ---
        private static void CapturarInfantil()
        {
            Console.WriteLine("\n=== REGISTRO PACIENTE INFANTIL ===");
            string nom = CapturadorDatos.LeerTextoOBLIGATORIO("Nombre del Paciente");
            int ed = CapturadorDatos.LeerEntero("Edad (años)", 0, 11);
            string exp = CapturadorDatos.LeerTextoOBLIGATORIO("Número de Expediente");
            string sx = CapturadorDatos.LeerTextoOBLIGATORIO("Género");
            string dir = CapturadorDatos.LeerTextoOBLIGATORIO("Dirección");
            string resp = CapturadorDatos.LeerTextoOBLIGATORIO("Nombre del Responsable");

            PacienteInfantil p = new PacienteInfantil(nom, ed, exp, sx, dir, resp);

            CapturarMotivoYSignos(p);
            CapturarExamenFisicoCompleto(p);

            Console.WriteLine("\n--- EVALUACIÓN DE MICRONUTRIENTES ---");
            p.VitaminaA = CapturadorDatos.LeerBooleano("Vitamina A");
            p.Multivitaminas = CapturadorDatos.LeerBooleano("Multivitaminas");
            p.Hierro = CapturadorDatos.LeerBooleano("Hierro");
            p.Mebendazol = CapturadorDatos.LeerBooleano("Mebendazol");
            p.Albendazol = CapturadorDatos.LeerBooleano("Albendazol");

            Console.WriteLine("\n--- VACUNACIÓN Y DERECHOS ---");
            p.EsquemaVacunacionCompleto = CapturadorDatos.LeerBooleano("Esquema de vacunación completo");
            p.SufreMaltratoOViolencia = CapturadorDatos.LeerBooleano("Sufre maltrato y/o violencia");
            p.CumpleDerechosNino = CapturadorDatos.LeerBooleano("Cumple con los derechos del niño/a");
            p.RiesgoSocial = CapturadorDatos.LeerBooleano("Riesgo social");

            CapturarDiagnosticoYTratamiento(p);
            gestor.RegistrarPaciente(p);
        }

        // --- SUBCLASE ADOLESCENTE ---
        private static void CapturarAdolescente()
        {
            Console.WriteLine("\n=== REGISTRO PACIENTE ADOLESCENTE ===");
            string nom = CapturadorDatos.LeerTextoOBLIGATORIO("Nombre del Paciente");
            int ed = CapturadorDatos.LeerEntero("Edad (años)", 12, 17);
            string exp = CapturadorDatos.LeerTextoOBLIGATORIO("Número de Expediente");
            string sx = CapturadorDatos.LeerTextoOBLIGATORIO("Género");
            string dir = CapturadorDatos.LeerTextoOBLIGATORIO("Dirección");
            string resp = CapturadorDatos.LeerTextoOBLIGATORIO("Responsable");

            PacienteAdolescente p = new PacienteAdolescente(nom, ed, exp, sx, dir, resp);

            CapturarMotivoYSignos(p);

            Console.WriteLine("\n--- ANTECEDENTES FAMILIARES PATOLÓGICOS ---");
            string[] antFam = new string[] { "Hipertensión arterial", "Trastornos mentales y de comportamiento", "Enfermedad renal crónica", "Infecciones", "Obesidad", "Otras enfermedades cardiovasculares", "Diabetes mellitus", "Asma bronquial", "Cáncer" };
            foreach (var item in antFam)
            {
                p.AntecedentesFamiliares[item] = CapturadorDatos.LeerBooleano($"Antecedente familiar: {item}");
            }

            Console.WriteLine("\n--- ANTECEDENTES PERSONALES PATOLÓGICOS ---");
            string[] antPers = new string[] { "Hipertensión arterial", "Otras enfermedades cardiovasculares", "Epilepsia", "EPOC", "Enfermedades infectocontagiosas", "Cáncer", "Asma bronquial", "Trastornos mentales", "Artritis", "Obesidad" };
            foreach (var item in antPers)
            {
                if (CapturadorDatos.LeerBooleano($"¿Padece {item}?"))
                {
                    int anio = CapturadorDatos.LeerEntero($"Año de inicio de {item}", 1900, DateTime.Now.Year);
                    string med = CapturadorDatos.LeerTextoOBLIGATORIO($"Medicamento recetado para {item}");
                    p.AntecedentesPersonales.Add(new AntecedentePersonal { Enfermedad = item, AnioInicio = anio, TipoMedication = med });
                }
            }

            Console.WriteLine("\n--- USO DE ALCOHOL Y DROGAS ---");
            p.Etilista = CapturadorDatos.LeerBooleano("Etilista");
            p.Tabaquista = CapturadorDatos.LeerBooleano("Tabaquista");
            p.Drogadiccion = CapturadorDatos.LeerBooleano("Drogadicción");

            CapturarExamenFisicoCompleto(p);

            Console.WriteLine("\n--- EVALUACIONES ADICIONALES ---");
            p.EscalaTanner = CapturadorDatos.LeerTextoOBLIGATORIO("Escala de Tanner");
            p.PanelSnellen = CapturadorDatos.LeerTextoOBLIGATORIO("Panel de Snellen");

            Console.WriteLine("\n--- SOLICITUD DE ESTUDIOS DE LABORATORIO ---");
            if (CapturadorDatos.LeerBooleano("¿Solicitar Hemograma automatizado (Sangre)?")) p.LaboratoriosSolicitados.Add("Hemograma automatizado (Muestra: Sangre)");
            if (CapturadorDatos.LeerBooleano("¿Solicitar Colesterol (Sangre)?")) p.LaboratoriosSolicitados.Add("Colesterol (Muestra: Sangre)");
            if (CapturadorDatos.LeerBooleano("¿Solicitar Triglicéridos (Sangre)?")) p.LaboratoriosSolicitados.Add("Triglicéridos (Muestra: Sangre)");
            if (CapturadorDatos.LeerBooleano("¿Solicitar General de Heces (Heces)?")) p.LaboratoriosSolicitados.Add("General de Heces (Muestra: Heces)");
            if (CapturadorDatos.LeerBooleano("¿Solicitar General de Orina (Orina)?")) p.LaboratoriosSolicitados.Add("General de Orina (Muestra: Orina)");

            CapturarDiagnosticoYTratamiento(p);
            gestor.RegistrarPaciente(p);
        }

        // --- SUBCLASE ADULTO MAYOR ---
        private static void CapturarAdultoMayor()
        {
            Console.WriteLine("\n=== REGISTRO PACIENTE ADULTO MAYOR ===");
            string nom = CapturadorDatos.LeerTextoOBLIGATORIO("Nombre del Paciente");
            int ed = CapturadorDatos.LeerEntero("Edad (años)", 60, 120);
            string exp = CapturadorDatos.LeerTextoOBLIGATORIO("Número de Expediente");
            string sx = CapturadorDatos.LeerTextoOBLIGATORIO("Género");
            string dir = CapturadorDatos.LeerTextoOBLIGATORIO("Dirección");
            string resp = CapturadorDatos.LeerTextoOBLIGATORIO("Cuidador/Responsable");

            PacienteAdultoMayor p = new PacienteAdultoMayor(nom, ed, exp, sx, dir, resp);

            CapturarMotivoYSignos(p);

            // Test de Barthel
            EvaluarIndiceBarthel(p);

            CapturarExamenFisicoCompleto(p);
            CapturarDiagnosticoYTratamiento(p);
            gestor.RegistrarPaciente(p);
        }

        // --- SUBCLASE PACIENTE CRÓNICO ---
        private static void CapturarCronico()
        {
            Console.WriteLine("\n=== REGISTRO PACIENTE CRÓNICO ===");
            string nom = CapturadorDatos.LeerTextoOBLIGATORIO("Nombre del Paciente");
            int ed = CapturadorDatos.LeerEntero("Edad (años)", 0, 120);
            string exp = CapturadorDatos.LeerTextoOBLIGATORIO("Número de Expediente");
            string sx = CapturadorDatos.LeerTextoOBLIGATORIO("Género");
            string dir = CapturadorDatos.LeerTextoOBLIGATORIO("Dirección");
            string resp = CapturadorDatos.LeerTextoOBLIGATORIO("Acompañante");

            PacienteCronico p = new PacienteCronico(nom, ed, exp, sx, dir, resp);

            CapturarMotivoYSignos(p);
            CapturarExamenFisicoCompleto(p);
            CapturarDiagnosticoYTratamiento(p);
            gestor.RegistrarPaciente(p);
        }

        // --- SUBCLASE MUJER EN EDAD FÉRTIL ---
        private static void CapturarMujerEdadFertil()
        {
            Console.WriteLine("\n=== REGISTRO PACIENTE MUJER EN EDAD FÉRTIL ===");
            string nom = CapturadorDatos.LeerTextoOBLIGATORIO("Nombre del Paciente");
            int ed = CapturadorDatos.LeerEntero("Edad (años)", 15, 49);
            string exp = CapturadorDatos.LeerTextoOBLIGATORIO("Número de Expediente");
            string dir = CapturadorDatos.LeerTextoOBLIGATORIO("Dirección");
            string resp = CapturadorDatos.LeerTextoOBLIGATORIO("Acompañante");

            PacienteMujerEdadFertil p = new PacienteMujerEdadFertil(nom, ed, exp, dir, resp);

            CapturarMotivoYSignos(p);

            Console.WriteLine("\n--- HISTORIA GINECO-OBSTÉTRICA PREVIA ---");
            int opMetodo = CapturadorDatos.LeerEntero("Método Anticonceptivo seleccionado (1 = Condón / 2 = Inyectable)", 1, 2);
            p.MetodoAnticonceptivo = (opMetodo == 1) ? "Condón" : "Inyectable";

            CapturarExamenFisicoCompleto(p);

            Console.WriteLine("\n--- DATOS ESPECÍFICOS DE MUJER EN EDAD FÉRTIL ---");
            p.FechaUltimaMenstruacion = CapturadorDatos.LeerTextoOBLIGATORIO("Fecha de última menstruación (FUM)");
            p.FechaUltimoParto = CapturadorDatos.LeerTextoOBLIGATORIO("Fecha de último parto (FUP)");
            p.FormulaObstetrica = CapturadorDatos.LeerTextoOBLIGATORIO("Fórmula Obstétrica (G:X P:X PP:X A:X V:X)");

            CapturarDiagnosticoYTratamiento(p);
            gestor.RegistrarPaciente(p);
        }

        // --- SUBCLASE EMBARAZADA ---
        private static void CapturarEmbarazada()
        {
            Console.WriteLine("\n=== REGISTRO PACIENTE EMBARAZADA ===");
            string nom = CapturadorDatos.LeerTextoOBLIGATORIO("Nombre del Paciente");
            int ed = CapturadorDatos.LeerEntero("Edad (años)", 10, 55);
            string exp = CapturadorDatos.LeerTextoOBLIGATORIO("Número de Expediente");
            string dir = CapturadorDatos.LeerTextoOBLIGATORIO("Dirección");
            string resp = CapturadorDatos.LeerTextoOBLIGATORIO("Acompañante");

            PacienteEmbarazada p = new PacienteEmbarazada(nom, ed, exp, dir, resp);

            CapturarMotivoYSignos(p);

            Console.WriteLine("\n--- HISTORIA OBSTÉTRICA PREVIA ---");
            p.FormulaObstetrica = CapturadorDatos.LeerTextoOBLIGATORIO("Fórmula Obstétrica (G:X P:X PP:X A:X V:X)");
            p.FinEmbarazoAnteriorFecha = CapturadorDatos.LeerTextoOBLIGATORIO("Fin Embarazo Anterior (Fecha/Año)");
            p.FinEmbarazoAnteriorTerminacion = CapturadorDatos.LeerTextoOBLIGATORIO("Terminación Embarazo Anterior (Parto/Cesárea/Aborto)");

            p.EmbarazoNoDeseado = CapturadorDatos.LeerBooleano("¿Embarazo no deseado?");
            p.UsabaMetodoAnticonceptivo = CapturadorDatos.LeerBooleano("¿Usaba método anticonceptivo al concebir?");
            if (p.UsabaMetodoAnticonceptivo)
                p.MetodoAnticonceptivoEmpleado = CapturadorDatos.LeerTextoOBLIGATORIO("Método anticonceptivo empleado");

            Console.WriteLine("\n--- DATOS DE GESTACIÓN ACTUAL ---");
            p.PesoAnteriorKg = CapturadorDatos.LeerDecimal("Peso Anterior/Inicial (kg, 30 - 200)", 30.0, 200.0);
            p.EdadGestacionalTexto = CapturadorDatos.LeerTextoOBLIGATORIO("Edad Gestacional (semanas de embarazo)");
            p.FUR = CapturadorDatos.LeerFecha("Fecha de Última Regla / Menstruación (FUR)");
            p.CalcularFPP();
            Console.WriteLine($" [Futuro Parto Probable]: {p.FPP.ToShortDateString()}");

            // Captura de Alcohol y Drogas por Trimestre
            Console.WriteLine("\n--- USO DE ALCOHOL Y DROGAS (POR TRIMESTRE) ---");
            CapturarTrimestre("Primer Trimestre", p.Trimestre1);
            CapturarTrimestre("Segundo Trimestre", p.Trimestre2);
            CapturarTrimestre("Tercer Trimestre", p.Trimestre3);

            // Captura de Vacunas
            Console.WriteLine("\n--- VACUNACIÓN ---");
            string[] vacunasNombres = { "Tétanos/difteria", "Tdpa", "Influenza", "Rubéola", "Hepatitis B", "Hepatitis A", "Covid" };
            foreach (var vac in vacunasNombres)
            {
              
               
                
                bool aplicada = CapturadorDatos.LeerBooleano($"¿Aplicada vacuna [{vac}]?");
                string fechaVac = "";
                if (aplicada)
                {
                    fechaVac = CapturadorDatos.LeerTextoOBLIGATORIO($"Fecha de aplicación de [{vac}]");
                }
                p.Vacunas.Add(new RegistroVacuna { NombreVacuna = vac, Aplicada = aplicada, FechaAplicacion = fechaVac });
            }

            // Captura de Perfiles Prenatales
            Console.WriteLine("\n--- PERFILES PRENATALES ---");
            p.Prenatal.CitologiaFechaToma = CapturadorDatos.LeerTextoOBLIGATORIO("Citología - Fecha de toma");
            p.Prenatal.CitologiaFechaResultado = CapturadorDatos.LeerTextoOBLIGATORIO("Citología - Fecha de resultado");
            p.Prenatal.CitologiaResultado = CapturadorDatos.LeerTextoOBLIGATORIO("Citología - Resultado (Normal/Anormal)");

            p.Prenatal.GrupoSanguineo = CapturadorDatos.LeerTextoOBLIGATORIO("Grupo Sanguíneo");

            p.Prenatal.HemoglobinaMenor20Sem = CapturadorDatos.LeerTextoOBLIGATORIO("Hemoglobina < 20 semanas (Fecha y Resultado (g/dL, 12.1 - 15.1))");
            p.Prenatal.HemoglobinaMayor20Sem = CapturadorDatos.LeerTextoOBLIGATORIO("Hemoglobina >= 20 semanas (Fecha y Resultado (g/dL, 12.1 - 15.1))");

            p.Prenatal.BacteriuriaMenor20Sem = CapturadorDatos.LeerTextoOBLIGATORIO("Bacteriuria < 20 semanas (Fecha y Resultado Normal/Anormal)");
            p.Prenatal.BacteriuriaMayor20Sem = CapturadorDatos.LeerTextoOBLIGATORIO("Bacteriuria >= 20 semanas (Fecha y Resultado Normal/Anormal)");

            p.Prenatal.GlucemiaAyunoMenor30Sem = CapturadorDatos.LeerTextoOBLIGATORIO("Glucemia Ayuno (<30 sem) (Fecha y Resultado (mg/dL, 70 - 99))");
            p.Prenatal.EstreptococosB_30_37Sem = CapturadorDatos.LeerTextoOBLIGATORIO("Estreptococos B (30-37 sem) (Positivo/Negativo/No se realizó)");

            bool vioVih = CapturadorDatos.LeerBooleano("¿Se realizó prueba de VIH?");
            if (vioVih)
            {
                string obsVihMenor = CapturadorDatos.LeerTextoOBLIGATORIO("VIH - Observaciones del resultado con < 20 semanas");
                string obsVihMayor = CapturadorDatos.LeerTextoOBLIGATORIO("VIH - Observaciones del resultado con >= 20 semanas");
                p.Prenatal.VIH = $"Realizada. <20sem: {obsVihMenor} | >=20sem: {obsVihMayor}";
            }
            else { p.Prenatal.VIH = "No se realizó"; }

            bool vioSifilis = CapturadorDatos.LeerBooleano("¿Se realizó prueba de Sífilis?");
            if (vioSifilis)
            {
                string obsSifMenor = CapturadorDatos.LeerTextoOBLIGATORIO("Sífilis - Observaciones < 20 semanas");
                string obsSifMayor = CapturadorDatos.LeerTextoOBLIGATORIO("Sífilis - Observaciones >= 20 semanas");
                p.Prenatal.Sifilis = $"Realizada. <20sem: {obsSifMenor} | >=20sem: {obsSifMayor}";
            }
            else { p.Prenatal.Sifilis = "No se realizó"; }

            p.Prenatal.UltrasonografiaObs = CapturadorDatos.LeerTextoOBLIGATORIO("Ultrasonografía Obstétrica (Observaciones del doctor)");

            // Solicitud de Laboratorios
            Console.WriteLine("\n--- SOLICITUD DE LABORATORIOS ---");
            string fechaLabDefault = CapturadorDatos.LeerTextoOBLIGATORIO("Fecha para la realización de laboratorios (Fecha de consulta)");
            string[] exLabs = {
        "HT - HB",
        "Serología de sífilis",
        "Urocultivo",
        "VIH",
        "Glucosa",
        "General de orina"
    };

            foreach (var ex in exLabs)
            {
                if (CapturadorDatos.LeerBooleano($"¿Solicitar examen [{ex}]?"))
                {
                    string muestra = (ex == "Urocultivo" || ex == "General de orina") ? "orina" : "sangre";
                    p.LaboratoriosEmbarazada.Add(new SolicitudLaboratorioEmbarazada
                    {
                        Fecha = fechaLabDefault,
                        NombreExamen = ex,
                        TipoMuestra = muestra
                    });
                }
            }

            CapturarExamenFisicoCompleto(p);
            CapturarDiagnosticoYTratamiento(p);
            gestor.RegistrarPaciente(p);
        }

        private static void CapturarTrimestre(string titulo, ConsumoYRiesgoTrimestre t)
        {
            Console.WriteLine($"-> {titulo}:");
            t.FumaActivamente = CapturadorDatos.LeerBooleano("  Fuma activamente");
            t.FumaPasivamente = CapturadorDatos.LeerBooleano("  Fuma pasivamente");
            t.Drogas = CapturadorDatos.LeerBooleano("  Drogas");
            t.Alcohol = CapturadorDatos.LeerBooleano("  Alcohol");
            t.Violencia = CapturadorDatos.LeerBooleano("  Violencia");
        }

        private static void DemostrarDestructor()
        {
            Console.WriteLine("\nLimpiando lista y forzando ejecución del Garbage Collector...");
            gestor.LimpiarLista();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Console.WriteLine("Memoria liberada exitosamente.");
        }
    }
}
