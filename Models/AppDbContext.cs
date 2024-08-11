using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;

namespace MVCproyect.Models
{
    public class AppDbContext
    {

        private readonly string? _connectionString;

        public AppDbContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        /*
         Explicación:
        
        Configuración: El constructor de AppDbContext recibe una instancia de IConfiguration, que es parte de la inyección de dependencias en .NET. Esto permite acceder a la configuración de la aplicación, como la cadena de conexión.
        
        CreateConnection: El método CreateConnection devuelve una conexión abierta a la base de datos utilizando SqlConnection, parte de ADO.NET. Puedes usar esta conexión para ejecutar comandos SQL.
         */

    }
}
