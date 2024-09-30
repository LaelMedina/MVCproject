using MVCproyect.Interfaces;
using MVCproyect.Models;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using ZstdSharp.Unsafe;

namespace MVCproyect.Services
{
    public class ProductService : IProductService
    {

        private readonly MySqlService _context;
        private List<Product> _products;

        public ProductService(MySqlService context)
        {
            _context = context;
            _products = new List<Product>();
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            try
            {
                using MySqlConnection connection = _context.CreateConnection();

                await connection.OpenAsync();

                string query = "SELECT id, name, price FROM products";

                using MySqlCommand command = new MySqlCommand(query, connection);

                using MySqlDataReader reader = (MySqlDataReader) await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    Product product = new Product()
                    {
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("name"),
                        Price = reader.GetDecimal("price")
                    };

                    _products.Add(product);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return _products;
        }

        public async Task UpdateStockAfterSale(int productId, int units)
        {
            try
            {
                using MySqlConnection connection = _context.CreateConnection();

                await connection.OpenAsync();

                string query = "UPDATE products SET stock = (stock - @units) WHERE id = @ProductId";

                using MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@ProductId", productId);

                command.Parameters.AddWithValue("@units", units);

                await command.ExecuteNonQueryAsync();

            }
            catch (Exception ex) 
            { 
                Console.WriteLine($"{ex.Message}"); 
            }
        }
    }
}
