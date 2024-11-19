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
        private readonly string _backupDirectory;

        public MySqlService(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _backupDirectory = Path.Combine(environment.ContentRootPath, "dbfile");

            if (!Directory.Exists(_backupDirectory))
            {
                Directory.CreateDirectory(_backupDirectory); 
            }
        }


        public MySqlConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public void DataBaseBackUp()
        {
            string fileName = $"productsdb_backup_{DateTime.Now:yyyyMMdd_HHmmss}.sql";
            string _path = Path.Combine(_backupDirectory, fileName);
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
            if (string.IsNullOrEmpty(_lastPath) || !File.Exists(_lastPath))
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