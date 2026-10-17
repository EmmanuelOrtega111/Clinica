using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Prueba_Proyecto_3
{
    public static class ValidadorClinico
    {
        public static void ValidarDatosGenerales(string expediente, string nombre, int edad, string sexo)
        {
            if (string.IsNullOrWhiteSpace(expediente))
            {
                throw new ExcepcionValidacionClinica("El Número de Expediente es obligatorio y no puede estar vacío.");
            }

            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ExcepcionValidacionClinica("El Nombre del paciente es obligatorio.");
            }

            if (edad < 0 || edad > 120)
            {
                throw new ExcepcionValidacionClinica($"La edad ingresada ({edad} años) no es válida. Debe estar en un rango de 0 a 120 años.");
            }

            if (string.IsNullOrWhiteSpace(sexo))
            {
                throw new ExcepcionValidacionClinica("Debe especificar el sexo del paciente.");
            }
        }

        public static void ValidarSignosVitales(double temperatura, int freqCardiaca, double peso, double talla)
        {
            if (temperatura > 0 && (temperatura < 30.0 || temperatura > 45.0))
            {
                throw new ExcepcionValidacionClinica($"La temperatura ({temperatura}°C) está fuera del rango biológico viable (30°C - 45°C).");
            }

            if (freqCardiaca < 0 || freqCardiaca > 250)
            {
                throw new ExcepcionValidacionClinica($"La frecuencia cardíaca ({freqCardiaca} lpm) no es realista.");
            }

            if (peso < 0 || peso > 500)
            {
                throw new ExcepcionValidacionClinica("El peso ingresado no es válido o excede el límite permitido.");
            }

            if (talla < 0 || talla > 250)
            {
                throw new ExcepcionValidacionClinica("La talla ingresada en centímetros no es válida.");
            }
        }
    }
}