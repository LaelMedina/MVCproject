using MVCproyect.Interfaces;
using MVCproyect.Models;
using MySql.Data.MySqlClient;

namespace MVCproyect.Services
{
    public class IdGeneratorService : IIdGeneratorService
    {
        private readonly MySqlService _context;

        public IdGeneratorService(MySqlService context)
        {
            _context = context;
        }

        public async Task<int> GenerateNextIdAsync(string tableName)
        {
            int newId = 1;

            try
            {
                using MySqlConnection connection = _context.CreateConnection();

                await connection.OpenAsync();

                string query = $"SELECT MAX(id) FROM {tableName}";

                using MySqlCommand command = new MySqlCommand(query, connection);

                var lastId = await command.ExecuteScalarAsync();

                if (lastId != DBNull.Value)
                {
                    newId = Convert.ToInt32(lastId) + 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error generating the new Id: " + ex.ToString());
            }

            return newId;
        }
    }
}
