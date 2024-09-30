using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MVCproyect.Models;
using System.Data;
using MVCproyect.Services;
using MVCproyect.Interfaces;

namespace MVCproyect.Controllers
{
    public class ProductController : Controller
    {
        private readonly MySqlService _context;
        private readonly IdGeneratorService _idGeneratorService;
        private readonly ProductRepository _productRepository;
        private List<Product> _products;

        public ProductController(MySqlService context, IdGeneratorService idGeneratorService, ProductRepository productRepository)
        {
            _context = context;
            _idGeneratorService = idGeneratorService;
            _products = new List<Product>();
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            _products = await _productRepository.GetProductsAsync();
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

            return RedirectToAction("Index");
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
                ViewBag.ErrorMessage = ex.Message;
                return View("ErrorMessage");
            }

            return View("Product");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateProduct(Product updatedProduct)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using MySqlConnection connection = _context.CreateConnection();

                    connection.Open();

                    string query = "UPDATE products SET name = @name, description = @description, price = @price, stock = @stock WHERE id = @id";

                    using MySqlCommand command = new MySqlCommand(query, connection);

                    command.Parameters.AddWithValue("@id", updatedProduct.Id);
                    command.Parameters.AddWithValue("@name", updatedProduct.Name);
                    command.Parameters.AddWithValue("@description", updatedProduct.Description);
                    command.Parameters.AddWithValue("@price", updatedProduct.Price);
                    command.Parameters.AddWithValue("@stock", updatedProduct.Stock);

                    command.ExecuteNonQuery();

                    ViewData["product"] = updatedProduct;
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "An error has occured: " + ex.Message.ToString() + "Product Id: " + updatedProduct.Id;
                    return View("ErrorView");
                }
            }


            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(int id)
        {
            try
            {
                using MySqlConnection connection = _context.CreateConnection();

                connection.Open();

                string query = "SELECT * FROM products WHERE id=@id";

                using MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();

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

                    ViewBag.currentProduct = product;

                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("ErrorMessage");
            }

            return View("EditProductForm");
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
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("ErrorMessage");
            }

            return RedirectToAction("Index");
        }
    }
}
