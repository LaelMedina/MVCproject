using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;

namespace MVCproyect.Services
{
    public class MySqlService
    {

        private readonly string? _connectionString;

        public MySqlService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public MySqlConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }

    }
}