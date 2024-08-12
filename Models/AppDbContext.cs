using Microsoft.Data.SqlClient;
using System.Data;

namespace MVCproyect.Models
{
    public class AppDbContext
    {

        private readonly string? _connectionString;

        public AppDbContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public SqlConnection CreateConnection() 
        {
            return new SqlConnection(_connectionString);
        }

    }
}
