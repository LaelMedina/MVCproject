using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MVCproyect.Models;
using System.Data;
using MVCproyect.Services;

namespace MVCproyect.Controllers
{
    public class ProductController : Controller
    {
        private readonly MySqlService _context;
        private readonly List<Product> _products;

        private readonly IdGeneratorService _idGeneratorService;

        public ProductController(MySqlService context)
        {
            _context = context;
            _products = new List<Product>();

            _idGeneratorService = new IdGeneratorService(_context);
        }

        public IActionResult Index()
        {
            try
            {
                using MySqlConnection connection = _context.CreateConnection();
                connection.Open();

                string query = "SELECT * FROM products";

                using MySqlCommand command = new MySqlCommand(query, connection);

                using MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Product product = new Product
                    {
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("name"),
                        Description = reader.GetString("description"),
                        Price = reader.GetDecimal("price"),
                        Stock = reader.GetInt32("stock"),
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product newProduct)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using MySqlConnection connection = _context.CreateConnection();

                    connection.Open();

                    string query = "INSERT INTO products (id, name, description, price, stock) VALUES (@id, @name, @description, @price, @stock)";

                    using MySqlCommand command = new MySqlCommand(query, connection);

                    newProduct.Id = _idGeneratorService.GenerateNextId("products");

                    command.Parameters.AddWithValue("@id", newProduct.Id);
                    command.Parameters.AddWithValue("@name", newProduct.Name);
                    command.Parameters.AddWithValue("@description", newProduct.Description);
                    command.Parameters.AddWithValue("@price", newProduct.Price);
                    command.Parameters.AddWithValue("@stock", newProduct.Stock);

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "An error has occured: " + ex.Message.ToString() + "Product Id: " + newProduct.Id;

                    return View("ErrorView");
                }
            }

            ViewData["Products"] = _products;
            return View("Index");
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
                        Stock = reader.GetInt32("stock"),
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

        public IActionResult Update(int id)
        {
            return View("ProductForm");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            using MySqlConnection connection = _context.CreateConnection();
            connection.Open();

            string query = "DELETE FROM products WHERE id=@id";

            using MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }

            return RedirectToAction("Index");
        }
    }
}
