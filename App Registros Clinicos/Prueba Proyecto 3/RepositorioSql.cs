using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SistemaUnidadSalud.Data;

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
                    //Guardar o actualizar en la tabla Pacientes
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

                    //Guardar datos de la Consulta
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

                    //Guardar Examen Físico
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

                    //Guardar Recetas
                    foreach (var med in nuevoPaciente.Receta)
                    {
                        string queryReceta = @"
                            INSERT INTO Recetas (IdConsulta, NombreMedicamento, CantidadYDosis)
                            VALUES (@IdConsulta, @NombreMedicamento, @CantidadYDosis)";

                        using (SqlCommand cmdReceta = new SqlCommand(queryReceta, conexion, transaccion))
                        {
                            cmdReceta.Parameters.AddWithValue("@IdConsulta", idConsulta);
                            cmdReceta.Parameters.AddWithValue("@NombreMedicamento", (object)med.Nombre ?? DBNull.Value);
                            cmdReceta.Parameters.AddWithValue("@CantidadYDosis", (object)med.CantidadYDosis ?? DBNull.Value);

                            cmdReceta.ExecuteNonQuery();
                        }
                    }

                    //Guardar Detalles de Subclases Específicas
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
                                IdConsulta, FormulaObstetrica, PesoAnteriorKg, EdadGestacionalTexto, 
                                FUR, FPP, GrupoSanguineo, VIH, Sifilis, UltrasonografiaObs
                            )
                            VALUES (
                                @IdConsulta, @FormulaObstetrica, @PesoAnteriorKg, @EdadGestacionalTexto, 
                                @FUR, @FPP, @GrupoSanguineo, @VIH, @Sifilis, @UltrasonografiaObs
                            )";

                        using (SqlCommand cmdEmb = new SqlCommand(queryEmbarazada, conexion, transaccion))
                        {
                            cmdEmb.Parameters.AddWithValue("@IdConsulta", idConsulta);
                            cmdEmb.Parameters.AddWithValue("@FormulaObstetrica", (object)embarazada.FormulaObstetrica ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@PesoAnteriorKg", embarazada.PesoAnteriorKg);
                            cmdEmb.Parameters.AddWithValue("@EdadGestacionalTexto", (object)embarazada.EdadGestacionalTexto ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@FUR", embarazada.FUR == DateTime.MinValue ? (object)DBNull.Value : embarazada.FUR);
                            cmdEmb.Parameters.AddWithValue("@FPP", embarazada.FPP == DateTime.MinValue ? (object)DBNull.Value : embarazada.FPP);
                            cmdEmb.Parameters.AddWithValue("@GrupoSanguineo", (object)embarazada.Prenatal?.GrupoSanguineo ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@VIH", (object)embarazada.Prenatal?.VIH ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@Sifilis", (object)embarazada.Prenatal?.Sifilis ?? DBNull.Value);
                            cmdEmb.Parameters.AddWithValue("@UltrasonografiaObs", (object)embarazada.Prenatal?.UltrasonografiaObs ?? DBNull.Value);

                            cmdEmb.ExecuteNonQuery();
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
                string query = "DELETE FROM Recetas; DELETE FROM ExamenesFisicos; DELETE FROM DetalleAdultoMayor; DELETE FROM DetalleEmbarazada; DELETE FROM Consultas; DELETE FROM Pacientes;";
                SqlCommand cmd = new SqlCommand(query, conexion);
                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}