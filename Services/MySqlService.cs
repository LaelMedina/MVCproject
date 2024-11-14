using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;

namespace MVCproyect.Services
{
    public class MySqlService
    {

        private readonly string? _connectionString;
        private string _lastPath;
        public MySqlService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public MySqlConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public void DataBaseBackUp() 
        {
            string fileName = $"productsdb_backup_{DateTime.Now:yyyyMMdd_HHmmss}.sql";
            string _path = "C:\\Users\\Usuario\\OneDrive\\Documentos\\Coding\\MVCproject\\dbfile\\" + fileName;
            _lastPath = _path;

            using (MySqlConnection connection = new MySqlConnection(_connectionString)) 
            {
                using (MySqlCommand command = new MySqlCommand()) 
                {
                    using (MySqlBackup backup = new MySqlBackup(command)) 
                    {
                        command.Connection = connection;

                        connection.Open();

                        backup.ExportToFile(_path);

                        connection.Close(); 
                    }
                }
            }

        }

        public void RestoreDataBase()
        {
            if (!File.Exists(_lastPath))
            {
                throw new FileNotFoundException("Backup file not found.", _lastPath);
            }

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    using (MySqlBackup restore = new MySqlBackup(command))
                    {
                        command.Connection = connection;

                        connection.Open();

                        restore.ImportFromFile(_lastPath);

                        connection.Close();
                    }
                }
            }
        }


    }
}