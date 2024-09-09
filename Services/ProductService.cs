using MVCproyect.Models;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using ZstdSharp.Unsafe;

namespace MVCproyect.Services
{
    public class ProductService
    {

        private readonly MySqlConnection _connection;
        private readonly List<Product> _products;

        public ProductService(MySqlService connection)
        {
            _connection = connection.CreateConnection();
            _products = new List<Product>();
        }

        public List<Product> GetProducts()
        {
            try
            {
                _connection.Open();

                string query = "SELECT id, name, price FROM products";

                using MySqlCommand command = new MySqlCommand(query, _connection);

                using MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
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

        public void UpdateStockAfterSale(int productId, int units)
        {
            try
            {
                _connection.Open();

                string query = "UPDATE products SET stock = (stock - @units) WHERE id = @ProductId";

                using MySqlCommand command = new MySqlCommand(query, _connection);

                command.Parameters.AddWithValue("@ProductId", productId);
                command.Parameters.AddWithValue("@units", units);

                command.ExecuteNonQuery();

            }
            catch (Exception ex) 
            { 
                Console.WriteLine($"{ex.Message}"); 
            }
        }

    }
}
