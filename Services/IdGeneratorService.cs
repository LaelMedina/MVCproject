using MVCproyect.Models;
using MySql.Data.MySqlClient;

namespace MVCproyect.Services
{
    public class IdGeneratorService
    {
        private readonly MySqlConnection _connection;

        public IdGeneratorService(MySqlService connection)
        {
            _connection = connection.CreateConnection();
        }

        public int GenerateNextId(string tableName)
        {
            int newId = 1;

            try
            {
                _connection.Open();

                string query = $"SELECT MAX(id) FROM {tableName}";

                using MySqlCommand command = new MySqlCommand(query, _connection);

                var lastId = command.ExecuteScalar();

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
