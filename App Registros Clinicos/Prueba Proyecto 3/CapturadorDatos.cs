using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_Proyecto_3
{
    public static class CapturadorDatos
    {
        public static string LeerTextoOBLIGATORIO(string mensaje)
        {
            string entrada = "";
            while (string.IsNullOrWhiteSpace(entrada))
            {
                Console.Write($"{mensaje}: ");
                entrada = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(entrada))
                {
                    Console.WriteLine("   [ERROR]: Este campo es obligatorio.");
                }
            }
            return entrada;
        }

        public static int LeerEntero(string mensaje, int minimo = 0, int maximo = 150)
        {
            int valor = 0;
            bool valido = false;
            while (!valido)
            {
                Console.Write($"{mensaje}: ");
                try
                {
                    valor = Convert.ToInt32(Console.ReadLine());
                    if (valor < minimo || valor > maximo)
                    {
                        throw new ArgumentOutOfRangeException($"El entero debe estar entre {minimo} y {maximo}.");
                    }
                    valido = true;
                }
                catch (FormatException)
                {
                    Console.WriteLine("   [ERROR FORMATO]: Ingrese un valor entero.");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine($"   [ERROR RANGO]: {ex.Message}");
                }
                finally
                {
                    if (!valido) Console.WriteLine("   --> Reintente.\n");
                }
            }
            return valor;
        }

        public static double LeerDecimal(string mensaje, double minimo = 0.0, double maximo = 500.0)
        {
            double valor = 0.0;
            bool valido = false;
            while (!valido)
            {
                Console.Write($"{mensaje}: ");
                try
                {
                    valor = Convert.ToDouble(Console.ReadLine());
                    if (valor < minimo || valor > maximo)
                    {
                        throw new ArgumentOutOfRangeException($"El valor debe estar entre {minimo} y {maximo}.");
                    }
                    valido = true;
                }
                catch (FormatException)
                {
                    Console.WriteLine("   [ERROR FORMATO]: Ingrese un número decimal (ej. 36.5).");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine($"   [ERROR RANGO]: {ex.Message}");
                }
                finally
                {
                    if (!valido) Console.WriteLine("   --> Reintente.\n");
                }
            }
            return valor;
        }

        public static bool LeerBooleano(string mensaje)
        {
            while (true)
            {
                Console.Write($"{mensaje} (S/N): ");
                string resp = Console.ReadLine().Trim().ToUpper();
                if (resp == "S") return true;
                if (resp == "N") return false;
                Console.WriteLine("   [ERROR]: Responda exclusivamente 'S' o 'N'.");
            }
        }

        public static DateTime LeerFecha(string mensaje)
        {
            DateTime fecha = DateTime.MinValue;
            bool valido = false;
            while (!valido)
            {
                Console.Write($"{mensaje} (dd/mm/yyyy): ");
                try
                {
                    fecha = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", null);
                    valido = true;
                }
                catch (FormatException)
                {
                    Console.WriteLine("   [ERROR FORMATO]: Respete el formato dd/mm/yyyy.");
                }
            }
            return fecha;
        }
    }
}
