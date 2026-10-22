using System.Data.SqlClient;

namespace SistemaUnidadSalud.Data
{
    public class ConexionBD
    {
        private readonly string cadenaConexion = @"Server=localhost; Database=SistemaUnidadSalud; Trusted_Connection=True; TrustServerCertificate=True;";

        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadenaConexion);
        }
    }
}