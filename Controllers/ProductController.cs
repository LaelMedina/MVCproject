using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;  // Asegúrate de usar el paquete correcto para MySQL
using MVCproyect.Models;
using System.Data;

namespace MVCproyect.Controllers
{
    public class ProductController : Controller
    {
        private readonly MySqlService _context;
        private readonly List<Product> _products;

        public ProductController(MySqlService context)
        {
            _context = context;
            _products = new List<Product>();
        }

        public IActionResult Index()
        {
            try
            {
                using MySqlConnection connection = _context.CreateConnection();  // Utiliza MySqlConnection
                connection.Open();

                string query = "SELECT * FROM products";

                using MySqlCommand command = new MySqlCommand(query, connection);  // Utiliza MySqlCommand

                using MySqlDataReader reader = command.ExecuteReader();  // Utiliza MySqlDataReader

                while (reader.Read())
                {
                    Product product = new Product
                    {
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("name"),
                        Description = reader.GetString("description"),
                        Price = reader.GetDecimal("price"),
                        CreatedAt = reader.GetDateTime("CreatedAt")
                    };
                    _products.Add(product);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

            ViewData["Products"] = _products;
            return View();
        }

        public IActionResult Create()
        {
            return View("ProductForm");
        }

        public IActionResult GetProductById(int id)
        {
            using MySqlConnection connection = _context.CreateConnection();
            connection.Open();

            try
            {
                string query = "SELECT * FROM products WHERE id=@id";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                using MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Product product = new Product
                    {
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("name"),
                        Description = reader.GetString("description"),
                        Price = reader.GetDecimal("price"),
                        CreatedAt = reader.GetDateTime("CreatedAt")
                    };

                    ViewData["product"] = product;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

            return View("Product");
        }

        public IActionResult Update()
        {
            return View("ProductForm");
        }

        public IActionResult Delete(int id)
        {
            using MySqlConnection connection = _context.CreateConnection();
            connection.Open();

            string query = "DELETE FROM products WHERE id=@id";

            using MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            try
            {
                command.ExecuteNonQuery();  // Solo ejecuta la consulta; no hay necesidad de usar un DataReader para DELETE
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }

            return RedirectToAction("Index");  // Redirige después de la eliminación en lugar de intentar mostrar el producto eliminado
        }
    }
}
