using SistemaUnidadSalud.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Prueba_Proyecto_3
{
    public class RepositorioSql : IRepositorioPaciente
    {
        private readonly ConexionBD _conexionBD = new ConexionBD();

        public void RegistrarPaciente(Paciente nuevoPaciente)
        {
            using (SqlConnection conexion = _conexionBD.ObtenerConexion())
            {
                conexion.Open();
                SqlTransaction transaccion = conexion.BeginTransaction();

                try
                {
                    // Guardar o actualizar en la tabla Pacientes
                    string queryPaciente = @"
                        IF NOT EXISTS (SELECT 1 FROM Pacientes WHERE NumeroExpediente = @NumeroExpediente)
                        BEGIN
                            INSERT INTO Pacientes (NumeroExpediente, Nombre, Edad, Sexo, Direccion, Responsable, TipoPaciente)
                            VALUES (@NumeroExpediente, @Nombre, @Edad, @Sexo, @Direccion, @Responsable, @TipoPaciente)
                        END
                        ELSE
                        BEGIN
                            UPDATE Pacientes 
                            SET Nombre = @Nombre, Edad = @Edad, Sexo = @Sexo, Direccion = @Direccion, Responsable = @Responsable, TipoPaciente = @TipoPaciente
                            WHERE NumeroExpediente = @NumeroExpediente
                        END";

                    using (SqlCommand cmdPaciente = new SqlCommand(queryPaciente, conexion, transaccion))
                    {
                        cmdPaciente.Parameters.AddWithValue("@NumeroExpediente", nuevoPaciente.NumeroExpediente);
                        cmdPaciente.Parameters.AddWithValue("@Nombre", nuevoPaciente.Nombre);
                        cmdPaciente.Parameters.AddWithValue("@Edad", nuevoPaciente.Edad);
                        cmdPaciente.Parameters.AddWithValue("@Sexo", nuevoPaciente.Sexo ?? (object)DBNull.Value);
                        cmdPaciente.Parameters.AddWithValue("@Direccion", nuevoPaciente.Direccion ?? (object)DBNull.Value);
                        cmdPaciente.Parameters.AddWithValue("@Responsable", nuevoPaciente.Responsable ?? (object)DBNull.Value);
                        cmdPaciente.Parameters.AddWithValue("@TipoPaciente", nuevoPaciente.GetType().Name);

                        cmdPaciente.ExecuteNonQuery();
                    }

                    // Guardar datos de la Consulta
                    string queryConsulta = @"
                        INSERT INTO Consultas (
                            NumeroExpediente, FechaConsulta, ConsultaPor, PresenteEnfermedad,
                            TemperaturaCelsius, FrecuenciaRespiratoria, FrecuenciaCardiaca, Pulso,
                            PresionArterial, SaturacionOxigeno, PesoKg, TallaCm, CircunferenciaAbdominalCm,
                            TipoDiagnostico, DiagnosticoTexto, TipoConsulta, Recomendaciones
                        )
                        OUTPUT INSERTED.IdConsulta
                        VALUES (
                            @NumeroExpediente, @FechaConsulta, @ConsultaPor, @PresenteEnfermedad,
                            @TemperaturaCelsius, @FrecuenciaRespiratoria, @FrecuenciaCardiaca, @Pulso,
                            @PresionArterial, @SaturacionOxigeno, @PesoKg, @TallaCm, @CircunferenciaAbdominalCm,
                            @TipoDiagnostico, @DiagnosticoTexto, @TipoConsulta, @Recomendaciones
                        )";

                    int idConsulta = 0;
                    using (SqlCommand cmdConsulta = new SqlCommand(queryConsulta, conexion, transaccion))
                    {
                        cmdConsulta.Parameters.AddWithValue("@NumeroExpediente", nuevoPaciente.NumeroExpediente);
                        cmdConsulta.Parameters.AddWithValue("@FechaConsulta", nuevoPaciente.FechaConsulta);
                        cmdConsulta.Parameters.AddWithValue("@ConsultaPor", (object)nuevoPaciente.ConsultaPor ?? DBNull.Value);
                        cmdConsulta.Parameters.AddWithValue("@PresenteEnfermedad", (object)nuevoPaciente.PresenteEnfermedad ?? DBNull.Value);
                        cmdConsulta.Parameters.AddWithValue("@TemperaturaCelsius", nuevoPaciente.TemperaturaCelsius);
                        cmdConsulta.Parameters.AddWithValue("@FrecuenciaRespiratoria", nuevoPaciente.FrecuenciaRespiratoria);
                        cmdConsulta.Parameters.AddWithValue("@FrecuenciaCardiaca", nuevoPaciente.FrecuenciaCardiaca);
                        cmdConsulta.Parameters.AddWithValue("@Pulso", nuevoPaciente.Pulso);
                        cmdConsulta.Parameters.AddWithValue("@PresionArterial", (object)nuevoPaciente.PresionArterial ?? DBNull.Value);
                        cmdConsulta.Parameters.AddWithValue("@SaturacionOxigeno", nuevoPaciente.SaturacionOxigeno);
                        cmdConsulta.Parameters.AddWithValue("@PesoKg", nuevoPaciente.PesoKg);
                        cmdConsulta.Parameters.AddWithValue("@TallaCm", nuevoPaciente.TallaCm);
                        cmdConsulta.Parameters.AddWithValue("@CircunferenciaAbdominalCm", nuevoPaciente.CircunferenciaAbdominalCm);
                        cmdConsulta.Parameters.AddWithValue("@TipoDiagnostico", (object)nuevoPaciente.TipoDiagnostico ?? DBNull.Value);
                        cmdConsulta.Parameters.AddWithValue("@DiagnosticoTexto", (object)nuevoPaciente.DiagnosticoTexto ?? DBNull.Value);
                        cmdConsulta.Parameters.AddWithValue("@TipoConsulta", (object)nuevoPaciente.TipoConsulta ?? DBNull.Value);
                        cmdConsulta.Parameters.AddWithValue("@Recomendaciones", (object)nuevoPaciente.Recomendaciones ?? DBNull.Value);

                        idConsulta = (int)cmdConsulta.ExecuteScalar();
                    }

                    // Guardar Examen Físico
                    foreach (var examen in nuevoPaciente.ExamenFisico)
                    {
                        string queryExamen = @"
                            INSERT INTO ExamenesFisicos (IdConsulta, Region, EsNormal, Detalle)
                            VALUES (@IdConsulta, @Region, @EsNormal, @Detalle)";

                        using (SqlCommand cmdExamen = new SqlCommand(queryExamen, conexion, transaccion))
                        {
                            cmdExamen.Parameters.AddWithValue("@IdConsulta", idConsulta);
                            cmdExamen.Parameters.AddWithValue("@Region", examen.Key);
                            cmdExamen.Parameters.AddWithValue("@EsNormal", examen.Value.EsNormal);
                            cmdExamen.Parameters.AddWithValue("@Detalle", (object)examen.Value.Detalle ?? DBNull.Value);

                            cmdExamen.ExecuteNonQuery();
                        }
                    }

                    // Guardar Recetas
                    foreach (var med in nuevoPaciente.Receta)
                    {
                         string queryReceta = @"
                           INSERT INTO Recetas (IdConsulta, IndicacionesMedicamento)
                           VALUES (@IdConsulta, @IndicacionesMedicamento)";

                        using (SqlCommand cmdReceta = new SqlCommand(queryReceta, conexion, transaccion))
                        {
                            cmdReceta.Parameters.AddWithValue("@IdConsulta", idConsulta);
                            cmdReceta.Parameters.AddWithValue("@IndicacionesMedicamento", (object)med.CantidadYDosis ?? DBNull.Value);

                            cmdReceta.ExecuteNonQuery();
                        }
                    }

                    // guardando los detalles de las subclases
                   
                    if (nuevoPaciente is PacienteAdultoMayor adultoMayor)
                    {
                        string queryAdultoMayor = @"
                            INSERT INTO DetalleAdultoMayor (IdConsulta, PuntajeBarthel, CategoriaDependencia)
                            VALUES (@IdConsulta, @PuntajeBarthel, @CategoriaDependencia)";

                        using (SqlCommand cmdAM = new SqlCommand(queryAdultoMayor, conexion, transaccion))
                        {
                            cmdAM.Parameters.AddWithValue("@IdConsulta", idConsulta);
                            cmdAM.Parameters.AddWithValue("@PuntajeBarthel", adultoMayor.PuntajeBarthel);
                            cmdAM.Parameters.AddWithValue("@CategoriaDependencia", (object)adultoMayor.CategoriaDependencia ?? DBNull.Value);

                            cmdAM.ExecuteNonQuery();
                        }
                    }
                    else if (nuevoPaciente is PacienteEmbarazada embarazada)
                    {
                          string queryEmbarazada = @"
                                 INSERT INTO DetalleEmbarazada (
                                 IdConsulta, 
                                 FormulaObstetrica, 
                                 FinEmbarazoAntFecha, 
                                 FinEmbarazoAntTerminacion, 
                                 EmbarazoNoDeseado, 
                                 UsabaMetodoAnticonceptivo, 
                                 MetodoAnticonceptivoEmpleado, 
                                 PesoAnteriorKg, 
                                 EdadGestacionalTexto, 
                                 FUR, 
                                 FPP,
                                 GrupoSanguineo, 
                                 PruebaVIH, 
                                 PruebaSifilis, 
                                 UltrasonografiaObs,
                                 -- Vacunas y fechas
                                 FechaTetanos, FechaTdpa, FechaInfluenza, FechaRubeola, FechaHepB, FechaHepA, FechaCovid,
                                 -- Perfil Prenatal y Exámenes
                                 CitologiaFechaToma, CitologiaFechaResultado, CitologiaResultado,
                                 HemoglobinaMenor20Sem, HemoglobinaMayor20Sem, 
                                 Chagas, 
                                 BacteriuriaMenor20Sem, BacteriuriaMayor20Sem, 
                                 GlucemiaAyunoMenor30Sem, 
                                 EstreptococosB_30_37Sem
                                                         )
                                 VALUES (
                                              @IdConsulta, 
                                              @FormulaObstetrica, 
                                              @FinEmbarazoAntFecha, 
                                              @FinEmbarazoAntTerminacion, 
                                              @EmbarazoNoDeseado, 
                                              @UsabaMetodoAnticonceptivo, 
                                              @MetodoAnticonceptivoEmpleado, 
                                              @PesoAnteriorKg, 
                                              @EdadGestacionalTexto, 
                                              @FUR, 
                                              @FPP,
                                              @GrupoSanguineo, 
                                              @VIH, 
                                              @Sifilis, 
                                              @UltrasonografiaObs,
                                              @FechaTetanos, @FechaTdpa, @FechaInfluenza, @FechaRubeola, @FechaHepB, @FechaHepA, @FechaCovid,
                                              @CitologiaFechaToma, @CitologiaFechaResultado, @CitologiaResultado,
                                              @HemoglobinaMenor20Sem, @HemoglobinaMayor20Sem, 
                                              @Chagas, 
                                              @BacteriuriaMenor20Sem, @BacteriuriaMayor20Sem, 
                                              @GlucemiaAyunoMenor30Sem, 
                                              @EstreptococosB_30_37Sem
                                        )";

                        using (SqlCommand cmdEmb = new SqlCommand(queryEmbarazada, conexion, transaccion))
                        {
                            cmdEmb.Parameters.AddWithValue("@IdConsulta", idConsulta);
                            cmdEmb.Parameters.AddWithValue("@FormulaObstetrica", (object)embarazada.FormulaObstetrica ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@FinEmbarazoAntFecha", (object)embarazada.FinEmbarazoAnteriorFecha ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@FinEmbarazoAntTerminacion", (object)embarazada.FinEmbarazoAnteriorTerminacion ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@EmbarazoNoDeseado", embarazada.EmbarazoNoDeseado);
                            cmdEmb.Parameters.AddWithValue("@UsabaMetodoAnticonceptivo", embarazada.UsabaMetodoAnticonceptivo);
                            cmdEmb.Parameters.AddWithValue("@MetodoAnticonceptivoEmpleado", (object)embarazada.MetodoAnticonceptivoEmpleado ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@PesoAnteriorKg", embarazada.PesoAnteriorKg);
                            cmdEmb.Parameters.AddWithValue("@EdadGestacionalTexto", (object)embarazada.EdadGestacionalTexto ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@FUR", embarazada.FUR == DateTime.MinValue ? (object)DBNull.Value : embarazada.FUR);
                            cmdEmb.Parameters.AddWithValue("@FPP", embarazada.FPP == DateTime.MinValue ? (object)DBNull.Value : embarazada.FPP);

                            // Datos del objeto Prenatal (con validación segura por si es nulo)
                            cmdEmb.Parameters.AddWithValue("@GrupoSanguineo", (object)embarazada.Prenatal?.GrupoSanguineo ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@VIH", (object)embarazada.Prenatal?.VIH ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@Sifilis", (object)embarazada.Prenatal?.Sifilis ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@UltrasonografiaObs", (object)embarazada.Prenatal?.UltrasonografiaObs ?? DBNull.Value);

                            // Fechas de Vacunas (capturadas directamente desde los TextBox de tu formulario de vacunas si las asignas, o de la lista de vacunas. Aquí mapeamos directo de las propiedades que recolectas)
                            cmdEmb.Parameters.AddWithValue("@FechaTetanos", (object)embarazada.Vacunas.FirstOrDefault(v => v.NombreVacuna.Contains("Tétanos"))?.FechaAplicacion ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@FechaTdpa", (object)embarazada.Vacunas.FirstOrDefault(v => v.NombreVacuna.Contains("Tdpa"))?.FechaAplicacion ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@FechaInfluenza", (object)embarazada.Vacunas.FirstOrDefault(v => v.NombreVacuna.Contains("Influenza"))?.FechaAplicacion ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@FechaRubeola", (object)embarazada.Vacunas.FirstOrDefault(v => v.NombreVacuna.Contains("Rubéola"))?.FechaAplicacion ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@FechaHepB", (object)embarazada.Vacunas.FirstOrDefault(v => v.NombreVacuna.Contains("Hepatitis B"))?.FechaAplicacion ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@FechaHepA", (object)embarazada.Vacunas.FirstOrDefault(v => v.NombreVacuna.Contains("Hepatitis A"))?.FechaAplicacion ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@FechaCovid", (object)embarazada.Vacunas.FirstOrDefault(v => v.NombreVacuna.Contains("Covid"))?.FechaAplicacion ?? DBNull.Value);

                            // Perfil Prenatal y Exámenes de laboratorio
                            cmdEmb.Parameters.AddWithValue("@CitologiaFechaToma", (object)embarazada.Prenatal?.CitologiaFechaToma ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@CitologiaFechaResultado", (object)embarazada.Prenatal?.CitologiaFechaResultado ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@CitologiaResultado", (object)embarazada.Prenatal?.CitologiaResultado ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@HemoglobinaMenor20Sem", (object)embarazada.Prenatal?.HemoglobinaMenor20Sem ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@HemoglobinaMayor20Sem", (object)embarazada.Prenatal?.HemoglobinaMayor20Sem ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@Chagas", (object)embarazada.Prenatal?.Chagas ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@BacteriuriaMenor20Sem", (object)embarazada.Prenatal?.BacteriuriaMenor20Sem ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@BacteriuriaMayor20Sem", (object)embarazada.Prenatal?.BacteriuriaMayor20Sem ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@GlucemiaAyunoMenor30Sem", (object)embarazada.Prenatal?.GlucemiaAyunoMenor30Sem ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@EstreptococosB_30_37Sem", (object)embarazada.Prenatal?.EstreptococosB_30_37Sem ?? DBNull.Value);

                            cmdEmb.ExecuteNonQuery();
                        }
                    }
                    else if (nuevoPaciente is PacienteInfantil infantil)
                    {
                        string queryInfantil = @"
                            INSERT INTO DetalleInfantil (
                                IdConsulta, VitaminaASuministrada, MultivitaminasSuministradas, HierroSuministrado, 
                                MebendazolSuministrado, AlbendazolSuministrado, EsquemaVacunacionCompleto, 
                                SufreMaltratoViolencia, CumpleDerechosNino, PresentaRiesgoSocial
                            )
                            VALUES (
                                @IdConsulta, @VitA, @Multiv, @Hierro, @Mebend, @Alben, @Esquema, @Maltrato, @Derechos, @Riesgo
                            )";

                        using (SqlCommand cmdInf = new SqlCommand(queryInfantil, conexion, transaccion))
                        {
                            cmdInf.Parameters.AddWithValue("@IdConsulta", idConsulta);
                            cmdInf.Parameters.AddWithValue("@VitA", infantil.VitaminaA);
                            cmdInf.Parameters.AddWithValue("@Multiv", infantil.Multivitaminas);
                            cmdInf.Parameters.AddWithValue("@Hierro", infantil.Hierro);
                            cmdInf.Parameters.AddWithValue("@Mebend", infantil.Mebendazol);
                            cmdInf.Parameters.AddWithValue("@Alben", infantil.Albendazol);
                            cmdInf.Parameters.AddWithValue("@Esquema", infantil.EsquemaVacunacionCompleto);
                            cmdInf.Parameters.AddWithValue("@Maltrato", infantil.SufreMaltratoOViolencia);
                            cmdInf.Parameters.AddWithValue("@Derechos", infantil.CumpleDerechosNino);
                            cmdInf.Parameters.AddWithValue("@Riesgo", infantil.RiesgoSocial);

                            cmdInf.ExecuteNonQuery();
                        }
                    }
                    else if (nuevoPaciente is PacienteAdolescente adolescente)
                    {
                        string queryAdolescente = @"
        INSERT INTO DetalleAdolescente (
            IdConsulta, Etilista, Tabaquista, Drogadiccion, EscalaTanner, PanelSnellen,
            
            -- Antecedentes Familiares
            AntFam_Hipertension, AntFam_TrastornosMentales, AntFam_RenalCronica, AntFam_Infecciones,
            AntFam_Obesidad, AntFam_Cardiovasculares, AntFam_Diabetes, AntFam_Asma, AntFam_Cancer,

            -- Antecedentes Personales (Año y Medicamento)
            AntPers_Hipertension_Anio, AntPers_Hipertension_Med,
            AntPers_Cardio_Anio, AntPers_Cardio_Med,
            AntPers_Epilepsia_Anio, AntPers_Epilepsia_Med,
            AntPers_EPOC_Anio, AntPers_EPOC_Med,
            AntPers_Infecto_Anio, AntPers_Infecto_Med,
            AntPers_Cancer_Anio, AntPers_Cancer_Med,
            AntPers_Asma_Anio, AntPers_Asma_Med,
            AntPers_Mentales_Anio, AntPers_Mentales_Med,
            AntPers_Artritis_Anio, AntPers_Artritis_Med,
            AntPers_Obesidad_Anio, AntPers_Obesidad_Med,

            -- Laboratorios Solicitados
            Lab_Hemograma, Lab_Colesterol, Lab_Trigliceridos, Lab_Heces, Lab_Orina
        )
        VALUES (
            @IdConsulta, @Etilista, @Tabaquista, @Drogadiccion, @EscalaTanner, @PanelSnellen,

            @AF_Hip, @AF_Ment, @AF_Ren, @AF_Inf, @AF_Obe, @AF_Card, @AF_Diab, @AF_Asma, @AF_Canc,

            @AP_Hip_A, @AP_Hip_M,
            @AP_Card_A, @AP_Card_M,
            @AP_Epi_A, @AP_Epi_M,
            @AP_EPOC_A, @AP_EPOC_M,
            @AP_Inf_A, @AP_Inf_M,
            @AP_Canc_A, @AP_Canc_M,
            @AP_Asma_A, @AP_Asma_M,
            @AP_Ment_A, @AP_Ment_M,
            @AP_Art_A, @AP_Art_M,
            @AP_Obe_A, @AP_Obe_M,

            @L_Hemo, @L_Col, @L_Trig, @L_Heces, @L_Orina
        )";

                        using (SqlCommand cmdAdol = new SqlCommand(queryAdolescente, conexion, transaccion))
                        {
                            cmdAdol.Parameters.AddWithValue("@IdConsulta", idConsulta);
                            cmdAdol.Parameters.AddWithValue("@Etilista", adolescente.Etilista);
                            cmdAdol.Parameters.AddWithValue("@Tabaquista", adolescente.Tabaquista);
                            cmdAdol.Parameters.AddWithValue("@Drogadiccion", adolescente.Drogadiccion);
                            cmdAdol.Parameters.AddWithValue("@EscalaTanner", (object)adolescente.EscalaTanner ?? DBNull.Value);
                            cmdAdol.Parameters.AddWithValue("@PanelSnellen", (object)adolescente.PanelSnellen ?? DBNull.Value);

                            // Mapeo de Antecedentes Familiares (del Dictionary<string, bool>)
                            cmdAdol.Parameters.AddWithValue("@AF_Hip", adolescente.AntecedentesFamiliares.TryGetValue("Hipertensión arterial", out bool af1) && af1);
                            cmdAdol.Parameters.AddWithValue("@AF_Ment", adolescente.AntecedentesFamiliares.TryGetValue("Trastornos mentales y de comportamiento", out bool af2) && af2);
                            cmdAdol.Parameters.AddWithValue("@AF_Ren", adolescente.AntecedentesFamiliares.TryGetValue("Enfermedad renal crónica", out bool af3) && af3);
                            cmdAdol.Parameters.AddWithValue("@AF_Inf", adolescente.AntecedentesFamiliares.TryGetValue("Infecciones", out bool af4) && af4);
                            cmdAdol.Parameters.AddWithValue("@AF_Obe", adolescente.AntecedentesFamiliares.TryGetValue("Obesidad", out bool af5) && af5);
                            cmdAdol.Parameters.AddWithValue("@AF_Card", adolescente.AntecedentesFamiliares.TryGetValue("Otras enfermedades cardiovasculares", out bool af6) && af6);
                            cmdAdol.Parameters.AddWithValue("@AF_Diab", adolescente.AntecedentesFamiliares.TryGetValue("Diabetes mellitus", out bool af7) && af7);
                            cmdAdol.Parameters.AddWithValue("@AF_Asma", adolescente.AntecedentesFamiliares.TryGetValue("Asma bronquial", out bool af8) && af8);
                            cmdAdol.Parameters.AddWithValue("@AF_Canc", adolescente.AntecedentesFamiliares.TryGetValue("Cáncer", out bool af9) && af9);

                            // Helper local para extraer Antecedentes Personales
                            AntecedentePersonal ObtenerAntPers(string enfermedad) =>
                                adolescente.AntecedentesPersonales.FirstOrDefault(ap => ap.Enfermedad == enfermedad);

                            void AgregarParametrosAntPers(string paramAnio, string paramMed, string enfermedad)
                            {
                                var ant = ObtenerAntPers(enfermedad);
                                if (ant != null)
                                {
                                    cmdAdol.Parameters.AddWithValue(paramAnio, ant.AnioInicio);
                                    cmdAdol.Parameters.AddWithValue(paramMed, (object)ant.TipoMedication ?? DBNull.Value);
                                }
                                else
                                {
                                    cmdAdol.Parameters.AddWithValue(paramAnio, DBNull.Value);
                                    cmdAdol.Parameters.AddWithValue(paramMed, DBNull.Value);
                                }
                            }

                            AgregarParametrosAntPers("@AP_Hip_A", "@AP_Hip_M", "Hipertensión arterial");
                            AgregarParametrosAntPers("@AP_Card_A", "@AP_Card_M", "Otras enfermedades cardiovasculares");
                            AgregarParametrosAntPers("@AP_Epi_A", "@AP_Epi_M", "Epilepsia");
                            AgregarParametrosAntPers("@AP_EPOC_A", "@AP_EPOC_M", "EPOC");
                            AgregarParametrosAntPers("@AP_Inf_A", "@AP_Inf_M", "Enfermedades infectocontagiosas");
                            AgregarParametrosAntPers("@AP_Canc_A", "@AP_Canc_M", "Cáncer");
                            AgregarParametrosAntPers("@AP_Asma_A", "@AP_Asma_M", "Asma bronquial");
                            AgregarParametrosAntPers("@AP_Ment_A", "@AP_Ment_M", "Trastornos mentales");
                            AgregarParametrosAntPers("@AP_Art_A", "@AP_Art_M", "Artritis");
                            AgregarParametrosAntPers("@AP_Obe_A", "@AP_Obe_M", "Obesidad");

                            // Mapeo de Laboratorios Solicitados (de la List<string>)
                            cmdAdol.Parameters.AddWithValue("@L_Hemo", adolescente.LaboratoriosSolicitados.Any(l => l.Contains("Hemograma")));
                            cmdAdol.Parameters.AddWithValue("@L_Col", adolescente.LaboratoriosSolicitados.Any(l => l.Contains("Colesterol")));
                            cmdAdol.Parameters.AddWithValue("@L_Trig", adolescente.LaboratoriosSolicitados.Any(l => l.Contains("Triglicéridos")));
                            cmdAdol.Parameters.AddWithValue("@L_Heces", adolescente.LaboratoriosSolicitados.Any(l => l.Contains("Heces")));
                            cmdAdol.Parameters.AddWithValue("@L_Orina", adolescente.LaboratoriosSolicitados.Any(l => l.Contains("Orina")));

                            cmdAdol.ExecuteNonQuery();
                        }
                    }
                    else if (nuevoPaciente is PacienteMujerEdadFertil mef)
                    {
                        string queryMef = @"
                            INSERT INTO DetalleMujerEdadFertil (IdConsulta, MetodoAnticonceptivo, FechaUltimaMenstruacion, FechaUltimoParto, FormulaObstetrica)
                            VALUES (@IdConsulta, @MetodoAnticonceptivo, @FechaUltimaMenstruacion, @FechaUltimoParto, @FormulaObstetrica)";

                        using (SqlCommand cmdMef = new SqlCommand(queryMef, conexion, transaccion))
                        {
                            cmdMef.Parameters.AddWithValue("@IdConsulta", idConsulta);
                            cmdMef.Parameters.AddWithValue("@MetodoAnticonceptivo", (object)mef.MetodoAnticonceptivo ?? DBNull.Value);
                            cmdMef.Parameters.AddWithValue("@FechaUltimaMenstruacion", (object)mef.FechaUltimaMenstruacion ?? DBNull.Value);
                            cmdMef.Parameters.AddWithValue("@FechaUltimoParto", (object)mef.FechaUltimoParto ?? DBNull.Value);
                            cmdMef.Parameters.AddWithValue("@FormulaObstetrica", (object)mef.FormulaObstetrica ?? DBNull.Value);

                            cmdMef.ExecuteNonQuery();
                        }
                    }

                    transaccion.Commit();
                    Console.WriteLine($"\n[ÉXITO ADO.NET]: Paciente {nuevoPaciente.Nombre} guardado correctamente.");
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    throw new Exception("Error al guardar mediante ADO.NET: " + ex.Message, ex);
                }
            }
        }

        public void MostrarTodos()
        {
            using (SqlConnection conexion = _conexionBD.ObtenerConexion())
            {
                string query = "SELECT NumeroExpediente, Nombre, Edad, Sexo, Direccion, Responsable, TipoPaciente FROM Pacientes";
                SqlCommand cmd = new SqlCommand(query, conexion);

                conexion.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Expediente: {reader["NumeroExpediente"]} | Nombre: {reader["Nombre"]} | Tipo: {reader["TipoPaciente"]}");
                    }
                }
            }
        }

        public Paciente BuscarPorExpediente(string numExpediente)
        {
            using (SqlConnection conexion = _conexionBD.ObtenerConexion())
            {
                string query = "SELECT NumeroExpediente, Nombre, Edad, Sexo, Direccion, Responsable, TipoPaciente FROM Pacientes WHERE NumeroExpediente = @Expediente";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@Expediente", numExpediente);

                conexion.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string tipo = reader["TipoPaciente"].ToString();
                        string nombre = reader["Nombre"].ToString();
                        int edad = Convert.ToInt32(reader["Edad"]);
                        string exp = reader["NumeroExpediente"].ToString();
                        string sexo = reader["Sexo"].ToString();
                        string dir = reader["Direccion"].ToString();
                        string resp = reader["Responsable"].ToString();

                        switch (tipo)
                        {
                            case nameof(PacienteInfantil):
                                return new PacienteInfantil(nombre, edad, exp, sexo, dir, resp);
                            case nameof(PacienteAdolescente):
                                return new PacienteAdolescente(nombre, edad, exp, sexo, dir, resp);
                            case nameof(PacienteAdultoMayor):
                                return new PacienteAdultoMayor(nombre, edad, exp, sexo, dir, resp);
                            case nameof(PacienteEmbarazada):
                                return new PacienteEmbarazada(nombre, edad, exp, dir, resp);
                            case nameof(PacienteMujerEdadFertil):
                                return new PacienteMujerEdadFertil(nombre, edad, exp, dir, resp);
                            default:
                                return new PacienteCronico(nombre, edad, exp, sexo, dir, resp);
                        }
                    }
                }
            }
            return null;
        }

        public void LimpiarLista()
        {
            using (SqlConnection conexion = _conexionBD.ObtenerConexion())
            {
                string query = "DELETE FROM Recetas; DELETE FROM ExamenesFisicos; DELETE FROM DetalleAdultoMayor; DELETE FROM DetalleEmbarazada; DELETE FROM DetalleInfantil; DELETE FROM DetalleAdolescente; DELETE FROM DetalleMujerEdadFertil; DELETE FROM Consultas; DELETE FROM Pacientes;";
                SqlCommand cmd = new SqlCommand(query, conexion);
                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}