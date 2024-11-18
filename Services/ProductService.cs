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
        private readonly ProductRepository _productRepository;
        private List<Product> _products;

        public ProductService(MySqlService context, ProductRepository productRepository)
        {
            _context = context;
            _productRepository = productRepository;
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

                using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

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

                // Verificar el stock actual
                string checkStockQuery = "SELECT stock FROM products WHERE id = @ProductId";
                using MySqlCommand checkStockCommand = new MySqlCommand(checkStockQuery, connection);
                checkStockCommand.Parameters.AddWithValue("@ProductId", productId);

                int currentStock = Convert.ToInt32(await checkStockCommand.ExecuteScalarAsync());

                if (currentStock < units)
                {
                    throw new InvalidOperationException($"Not enough stock for product {productId}. Available: {currentStock}, Requested: {units}");
                }

                // Actualizar el stock
                string updateStockQuery = "UPDATE products SET stock = stock - @units WHERE id = @ProductId";
                using MySqlCommand updateCommand = new MySqlCommand(updateStockQuery, connection);
                updateCommand.Parameters.AddWithValue("@ProductId", productId);
                updateCommand.Parameters.AddWithValue("@units", units);

                await updateCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }
    }

}
