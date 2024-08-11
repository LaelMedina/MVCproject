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

    }
}
