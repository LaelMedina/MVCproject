using MVCproyect.Models;
using MySql.Data.MySqlClient;
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

                string query = "SELECT id, name FROM products";

                using MySqlCommand command = new MySqlCommand(query, _connection);

                using MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Product product = new Product()
                    {
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("name"),
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

    }
}
