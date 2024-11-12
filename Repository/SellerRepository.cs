using MVCproyect.Interfaces;
using MVCproyect.Models;
using MVCproyect.Services;
using MySql.Data.MySqlClient;

namespace MVCproyect.Repository
{
    public class SellerRepository
    {
        private MySqlService _context;
        public SellerRepository(MySqlService context) 
        {
            _context = context;
        }

        public async Task<List<Seller>> GetSellersAsync() 
        {
            List<Seller> sellers = new List<Seller>();

            try
            {
                using (MySqlConnection connection = _context.CreateConnection())
                {
                    await connection.OpenAsync();

                    string query = "SELECT * FROM tb_seller";

                    using MySqlCommand command = new MySqlCommand(query, connection);

                    using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        sellers.Add(new Seller
                        {
                            Id = reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                            Identity = reader.GetString("Identity"),
                            CreatedOn = reader.GetDateTime("CreatedOn")
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return sellers;
        }

        public async Task<Seller> GetSellerByIdAsync(int id)
        {
            Seller seller = new Seller();

            try
            {
                using MySqlConnection connection = _context.CreateConnection();

                await connection.OpenAsync();

                string query = "SELECT Id, Name, Identity FROM tb_seller WHERE Id=@id";

                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@id", id);

                using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    seller = new Seller
                    {
                        Id = reader.GetInt32("Id"),
                        Name = reader.GetString("Name"),
                        Identity = reader.GetString("Identity"),
                        CreatedOn = reader.GetDateTime("CreatedOn")
                    };
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return seller;
        }

        public async Task AddSellerAsync(Seller newSeller)
        {
            try
            {
                using MySqlConnection connection = _context.CreateConnection();

                await connection.OpenAsync();

                string query = "INSERT INTO tb_seller (Name, Identity, CreatedOn) VALUES (@name, @identity, @createdOn)";

                using MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@name", newSeller.Name);
                command.Parameters.AddWithValue("@identity", newSeller.Identity);
                command.Parameters.AddWithValue("@createdOn", newSeller.CreatedOn);

                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task DeleteSellerAsync(int id)
        {
            try
            {
                using MySqlConnection connection = _context.CreateConnection();

                await connection.OpenAsync();

                string query = "DELETE FROM tb_seller WHERE Id=@id";

                using MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@id", id);

                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //Not Implemented Yet
        public Task UpdateSellerAsync(Seller newSeller)
        {
            throw new NotImplementedException();
        }
    }
}
