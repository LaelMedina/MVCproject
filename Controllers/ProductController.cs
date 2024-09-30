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
        public async Task<IActionResult> Create(Product newProduct)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _productRepository.AddProductAsync(newProduct);
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "An error has occured: " + ex.Message.ToString() + "Product Id: " + newProduct.Id;
                    return View("ErrorView");
                }
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetProductById(int id)
        {
            Product product = await _productRepository.GetProductByIdAsync(id);
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
