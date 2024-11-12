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
                            SellerId = reader.GetInt32("SellerId"),
                            SellerName = reader.GetString("SellerName"),
                            TotalSale = reader.GetDecimal("TotalSale"),
                            Currency = reader.GetString("Currency"),
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

        public async Task<List<Sale>> GetSalesWithDetailsAsync()
        {
            List<Sale> salesList = new List<Sale>();

            try
            {
                using MySqlConnection connection = _context.CreateConnection();
                await connection.OpenAsync();

                string query = "SELECT * FROM sales";

                using MySqlCommand command = new MySqlCommand(query, connection);
                using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var sale = new Sale
                    {
                        Id = reader.GetInt32("Id"),
                        ClientName = reader.GetString("ClientName"),
                        SellerId = reader.GetInt32("SellerId"),
                        SellerName = reader.GetString("SellerName"),
                        TotalSale = reader.GetDecimal("TotalSale"),
                        Currency = reader.GetString("Currency"),
                        PaymentMethod = reader.GetString("PaymentMethod"),
                        CreatedAt = reader.GetDateTime("CreatedAt"),
                        SaleDetails = new List<SaleDetail>()
                    };

                    salesList.Add(sale);
                }
                reader.Close();

                if (salesList.Any())
                {
                    var saleIds = string.Join(",", salesList.Select(s => s.Id));

                    string detailsQuery = $"SELECT * FROM sale_details WHERE SaleId IN ({saleIds})";

                    using MySqlCommand detailsCommand = new MySqlCommand(detailsQuery, connection);
                    using MySqlDataReader detailsReader = (MySqlDataReader)await detailsCommand.ExecuteReaderAsync();

                    while (await detailsReader.ReadAsync())
                    {
                        var detail = new SaleDetail()
                        {
                            Id = detailsReader.GetInt32("Id"),
                            SaleId = detailsReader.GetInt32("SaleId"),
                            ProductId = detailsReader.GetInt32("ProductId"),
                            ProductName = detailsReader.GetString("ProductName"),
                            Price = detailsReader.GetDecimal("Price"),
                            Units = detailsReader.GetInt32("Units"),
                        };

                        var sale = salesList.FirstOrDefault(s => s.Id == detail.SaleId);
                        if (sale != null)
                        {
                            sale.SaleDetails.Add(detail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return salesList;
        }

        public async Task<Sale> GetSaleByIdAsync(int id)
        {
            Sale sale = null;

            try
            {
                using MySqlConnection connection = _context.CreateConnection();
                await connection.OpenAsync();

                string query = "SELECT * FROM sales WHERE Id = @id";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    sale = new Sale()
                    {
                        Id = reader.GetInt32("Id"),
                        ClientName = reader.GetString("ClientName"),
                        SellerId = reader.GetInt32("SellerId"),
                        SellerName = reader.GetString("SellerName"),
                        TotalSale = reader.GetDecimal("TotalSale"),
                        Currency = reader.GetString("Currency"),
                        PaymentMethod = reader.GetString("PaymentMethod"),
                        CreatedAt = reader.GetDateTime("CreatedAt"),
                        SaleDetails = new List<SaleDetail>()
                    };
                }
                reader.Close();

                if (sale != null)
                {
                    string detailsQuery = "SELECT * FROM sale_details WHERE SaleId = @saleId";

                    using MySqlCommand detailsCommand = new MySqlCommand(detailsQuery, connection);
                    detailsCommand.Parameters.AddWithValue("@saleId", sale.Id);

                    using MySqlDataReader detailsReader = (MySqlDataReader)await detailsCommand.ExecuteReaderAsync();

                    while (await detailsReader.ReadAsync())
                    {
                        var detail = new SaleDetail()
                        {
                            Id = detailsReader.GetInt32("Id"),
                            SaleId = detailsReader.GetInt32("SaleId"),
                            ProductId = detailsReader.GetInt32("ProductId"),
                            ProductName = detailsReader.GetString("ProductName"),
                            Price = detailsReader.GetDecimal("Price"),
                            Units = detailsReader.GetInt32("Units"),
                        };

                        sale.SaleDetails.Add(detail);
                    }
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

                using var transaction = await connection.BeginTransactionAsync();

                string saleQuery = "INSERT INTO sales (id, clientname, SellerId, SellerName, totalsale, currency, paymentmethod) VALUES (@Id, @ClientName, @SellerId, @SellerName, @TotalSale, @Currency, @PaymentMethod)";
                using MySqlCommand saleCommand = new MySqlCommand(saleQuery, connection, transaction);
                newSale.Id = await _idGeneratorService.GenerateNextIdAsync("sales");
                saleCommand.Parameters.AddWithValue("@Id", newSale.Id);
                saleCommand.Parameters.AddWithValue("@ClientName", newSale.ClientName);
                saleCommand.Parameters.AddWithValue("@SellerId", newSale.SellerId);
                saleCommand.Parameters.AddWithValue("@SellerName", newSale.SellerName);
                saleCommand.Parameters.AddWithValue("@TotalSale", newSale.TotalSale);
                saleCommand.Parameters.AddWithValue("@Currency", newSale.Currency);
                saleCommand.Parameters.AddWithValue("@PaymentMethod", newSale.PaymentMethod);
                await saleCommand.ExecuteNonQueryAsync();

                string productQuery = "INSERT INTO sale_details (SaleId, ProductId, ProductName, Price, Units) VALUES (@SaleId, @ProductId, @ProductName, @Price, @Units)";
                foreach (var detail in newSale.SaleDetails)
                {
                    using MySqlCommand productCommand = new MySqlCommand(productQuery, connection, transaction);
                    productCommand.Parameters.AddWithValue("@SaleId", newSale.Id);
                    productCommand.Parameters.AddWithValue("@ProductId", detail.ProductId);
                    productCommand.Parameters.AddWithValue("@ProductName", detail.ProductName);
                    productCommand.Parameters.AddWithValue("@Price", detail.Price);
                    productCommand.Parameters.AddWithValue("@Units", detail.Units);
                    await productCommand.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
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

                using MySqlTransaction transaction = await connection.BeginTransactionAsync();

                try
                {
                    // Actualizar la venta principal en `sales`
                    string updateSaleQuery = @" UPDATE sales SET clientname = @ClientName, SellerId = @SellerId, SellerName = @SellerName, totalsale = @TotalSale, Currency = @Currency, paymentmethod = @PaymentMethod WHERE id = @Id";

                    using MySqlCommand saleCommand = new MySqlCommand(updateSaleQuery, connection, transaction);
                    saleCommand.Parameters.AddWithValue("@Id", updatedSale.Id);
                    saleCommand.Parameters.AddWithValue("@ClientName", updatedSale.ClientName);
                    saleCommand.Parameters.AddWithValue("@SellerId", updatedSale.SellerId);
                    saleCommand.Parameters.AddWithValue("@SellerName", updatedSale.SellerName);
                    saleCommand.Parameters.AddWithValue("@TotalSale", updatedSale.TotalSale);
                    saleCommand.Parameters.AddWithValue("@Currency", updatedSale.Currency);
                    saleCommand.Parameters.AddWithValue("@PaymentMethod", updatedSale.PaymentMethod);

                    await saleCommand.ExecuteNonQueryAsync();

                    // Actualizar o insertar detalles de la venta en `sale_details`
                    // Primero elimina los detalles anteriores de `sale_details` para esta venta
                    string deleteDetailsQuery = "DELETE FROM sale_details WHERE SaleId = @SaleId";
                    using MySqlCommand deleteCommand = new MySqlCommand(deleteDetailsQuery, connection, transaction);
                    deleteCommand.Parameters.AddWithValue("@SaleId", updatedSale.Id);
                    await deleteCommand.ExecuteNonQueryAsync();

                    foreach (var detail in updatedSale.SaleDetails)
                    {
                        string insertDetailQuery = @"
                    INSERT INTO sale_details (SaleId, ProductId, ProductName, Price, Units) 
                    VALUES (@SaleId, @ProductId, @ProductName, @Price, @Units)";

                        using MySqlCommand detailCommand = new MySqlCommand(insertDetailQuery, connection, transaction);
                        detailCommand.Parameters.AddWithValue("@SaleId", updatedSale.Id);
                        detailCommand.Parameters.AddWithValue("@ProductId", detail.ProductId);
                        detailCommand.Parameters.AddWithValue("@ProductName", detail.ProductName);
                        detailCommand.Parameters.AddWithValue("@Price", detail.Price);
                        detailCommand.Parameters.AddWithValue("@Units", detail.Units);

                        await detailCommand.ExecuteNonQueryAsync();
                    }

                    await transaction.CommitAsync();
                }
                    catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"Error: {ex.Message}");
                    throw;
                }
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
