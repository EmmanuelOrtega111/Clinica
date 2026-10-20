using System;

namespace Prueba_Proyecto_3
{
    public static class ValidadorClinico
    {
        //Validar Datos Generales Básicos
        public static void ValidarDatosGenerales(string expediente, string nombre, int edad, string sexo)
        {
            if (string.IsNullOrWhiteSpace(expediente))
            {
                throw new ExcepcionValidacionClinica("El 'Número de Expediente' es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ExcepcionValidacionClinica("El 'Nombre del Paciente' es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(sexo))
            {
                throw new ExcepcionValidacionClinica("Debe seleccionar el 'Sexo' o 'Género' del paciente.");
            }
        }

        //Validar Motivo de Consulta
        public static void ValidarMotivoConsulta(string consultaPor, string presenteEnfermedad)
        {
            if (string.IsNullOrWhiteSpace(consultaPor))
            {
                throw new ExcepcionValidacionClinica("El campo 'Consulta Por' es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(presenteEnfermedad))
            {
                throw new ExcepcionValidacionClinica("El campo 'Presente Enfermedad' es obligatorio.");
            }
        }

        //Validar Signos Vitales y Medidas Antropométricas
        public static void ValidarSignosVitales(
            double temp, int fr, int fc, int pulso, string pa, double spo2, double peso, double talla, double cirAbdomen)
        {
            if (temp < 30.0 || temp > 45.0)
            {
                throw new ExcepcionValidacionClinica($"La temperatura ({temp}°C) no está en un rango biológico válido (30°C - 45°C).");
            }

            if (fr < 5 || fr > 100)
            {
                throw new ExcepcionValidacionClinica($"La frecuencia respiratoria ({fr} rpm) no es válida (Rango: 5 - 100 rpm).");
            }

            if (fc < 30 || fc > 250)
            {
                throw new ExcepcionValidacionClinica($"La frecuencia cardíaca ({fc} lpm) no es válida (Rango: 30 - 250 lpm).");
            }

            if (pulso < 30 || pulso > 250)
            {
                throw new ExcepcionValidacionClinica($"El pulso ({pulso} ppm) no es válido (Rango: 30 - 250 ppm).");
            }

            if (string.IsNullOrWhiteSpace(pa) || !pa.Contains("/"))
            {
                throw new ExcepcionValidacionClinica("La Presión Arterial debe tener un formato válido (ej. 120/80).");
            }

            if (spo2 < 50.0 || spo2 > 100.0)
            {
                throw new ExcepcionValidacionClinica($"La saturación de oxígeno ({spo2}%) no es válida (Rango: 50% - 100%).");
            }

            if (peso <= 0 || peso > 400.0)
            {
                throw new ExcepcionValidacionClinica($"El peso ({peso} kg) no es válido.");
            }

            if (talla <= 0 || talla > 250.0)
            {
                throw new ExcepcionValidacionClinica($"La talla ({talla} cm) no es válida.");
            }

            if (cirAbdomen < 0 || cirAbdomen > 300.0)
            {
                throw new ExcepcionValidacionClinica("La circunferencia abdominal ingresada no es válida.");
            }
        }
    }
}