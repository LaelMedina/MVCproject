using MVCproyect.Interfaces;
using MVCproyect.Models;
using MVCproyect.Services;
using MySql.Data.MySqlClient;

namespace MVCproyect.Repository
{
    public class SaleRepository : ISaleRepository
    {

        private readonly MySqlService _context;
        private readonly IdGeneratorService _idGeneratorService;

        public SaleRepository(MySqlService context, IdGeneratorService idGeneratorService)
        {
            _context = context;
            _idGeneratorService = idGeneratorService;
        }

        public async Task<List<Sale>> GetSalesAsync()
        {
            List<Sale> sales = new List<Sale>();

            try
            {
                using (MySqlConnection connection = _context.CreateConnection())
                {
                    await connection.OpenAsync();

                    string query = "SELECT * FROM sales";

                    using MySqlCommand command = new MySqlCommand(query, connection);

                    using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        sales.Add(new Sale
                        {
                            Id = reader.GetInt32("Id"),
                            ClientName = reader.GetString("ClientName"),
                            SaleContent = reader.GetString("SaleContent"),
                            ProductSoldId = reader.GetInt32("ProductSoldId"),
                            TotalUnits = reader.GetInt32("TotalUnits"),
                            TotalSale = reader.GetDecimal("TotalSale"),
                            Currency = reader.GetInt32("Currency"),
                            PaymentMethod = reader.GetString("PaymentMethod"),
                            CreatedAt = reader.GetDateTime("CreatedAt")
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return sales;
        }

        public async Task<Sale> GetSaleByIdAsync(int id)
        {
            Sale sale = new Sale();

            try
            {
                using MySqlConnection connection = _context.CreateConnection();

                await connection.OpenAsync();

                string query = "SELECT * FROM sales WHERE id=@id";

                using MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@id", id);

                using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    sale = new Sale()
                    {
                        Id = reader.GetInt32("Id"),
                        ClientName = reader.GetString("ClientName"),
                        SaleContent = reader.GetString("SaleContent"),
                        ProductSoldId = reader.GetInt32("ProductSoldId"),
                        TotalUnits = reader.GetInt32("TotalUnits"),
                        TotalSale = reader.GetDecimal("TotalSale"),
                        Currency = reader.GetInt32("Currency"),
                        PaymentMethod = reader.GetString("PaymentMethod"),
                        CreatedAt = reader.GetDateTime("CreatedAt")
                    };

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return sale;
        }

        public async Task AddSaleAsync(Sale newSale)
        {
            try
            {
                using MySqlConnection connection = _context.CreateConnection();

                await connection.OpenAsync();

                string query = "INSERT INTO sales (id, clientname, salecontent, productsoldid, totalunits, totalsale, Currency, paymentmethod) VALUES (@Id, @ClientName, @SaleContent, @ProductSoldId, @TotalUnits, @TotalSale, @Currency, @PaymentMethod)";

                using MySqlCommand command = new MySqlCommand(query, connection);

                newSale.Id = await _idGeneratorService.GenerateNextIdAsync("sales");

                command.Parameters.AddWithValue("@Id", newSale.Id);
                command.Parameters.AddWithValue("@ClientName", newSale.ClientName);
                command.Parameters.AddWithValue("@SaleContent", newSale.SaleContent);
                command.Parameters.AddWithValue("@ProductSoldId", newSale.ProductSoldId);
                command.Parameters.AddWithValue("@TotalUnits", newSale.TotalUnits);
                command.Parameters.AddWithValue("@TotalSale", newSale.TotalSale);
                command.Parameters.AddWithValue("@Currency", newSale.Currency);
                command.Parameters.AddWithValue("@PaymentMethod", newSale.PaymentMethod);

                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task UpdateSaleAsync(Sale updatedSale)
        {
            try
            {
                using MySqlConnection connection = _context.CreateConnection();

                await connection.OpenAsync();

                string query = "UPDATE sales SET clientname = @ClientName, salecontent = @SaleContent, productsoldid = @ProductSoldId, totalunits = @TotalUnits, totalsale = @TotalSale, Currency = @Currency, paymentmethod = @PaymentMethod WHERE id = @Id";

                using MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", updatedSale.Id);
                command.Parameters.AddWithValue("@ClientName", updatedSale.ClientName);
                command.Parameters.AddWithValue("@SaleContent", updatedSale.SaleContent);
                command.Parameters.AddWithValue("@ProductSoldId", updatedSale.ProductSoldId);
                command.Parameters.AddWithValue("@TotalUnits", updatedSale.TotalUnits);
                command.Parameters.AddWithValue("@TotalSale", updatedSale.TotalSale);
                command.Parameters.AddWithValue("@Currency", updatedSale.Currency);
                command.Parameters.AddWithValue("@PaymentMethod", updatedSale.PaymentMethod);

                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task DeleteSaleAsync(int id)
        {
            try
            {
                using MySqlConnection connection = _context.CreateConnection();

                await connection.OpenAsync();

                string query = "DELETE FROM sales WHERE id=@id";

                using MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@id", id);

                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
