using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SistemaUnidadSalud.Data
{
    public class ConexionBD
    {
        private readonly string cadenaConexion = "Server=localhost; Database=SistemaUnidadSalud; Trusted_Connection=True;";
        public SqlConnection ObtenerConexion()
        {
            SqlConnection conexion = new SqlConnection(cadenaConexion);
            return conexion;
        }
    }
}