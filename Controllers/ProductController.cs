using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MVCproyect.Models;
using System.Data;

namespace MVCproyect.Controllers
{
    public class ProductController : Controller
    {

        private readonly AppDbContext _context;

        private readonly List<Product> _products;

        public ProductController(AppDbContext context)
        {
            _context = context;
            _products = new List<Product>();
        }

        public IActionResult Index()
        {
            try
            {
                SqlConnection connection = _context.CreateConnection();

                connection.Open();

                String query = "SELECT * FROM products";

                using SqlCommand command = new SqlCommand(query, connection);

                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Product product = new Product
                    {
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("name"),
                        Description = reader.GetString("description"),
                        Price = reader.GetDecimal("price"),
                        CreatedAt = reader.GetDateTime("created_at")

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

        public IActionResult getProductById(int id)
        {

            SqlConnection connection = _context.CreateConnection();

            connection.Open();

            try
            {
                string query = $"SELECT * FROM products WHERE id=@id";

                using SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@id", id);

                using SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Product product = new Product
                    {
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("name"),
                        Description = reader.GetString("description"),
                        Price = reader.GetDecimal("price"),
                        CreatedAt = reader.GetDateTime("created_at")
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
            SqlConnection connection = _context.CreateConnection();

            connection.Open();

            string query = "DELETE FROM products WHERE id=@id";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", id);

            try
            {
                using SqlDataReader reader = command.ExecuteReader();

                if (reader.Read()) 
                {
                    Product deletedProduct = new Product
                    {
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("name"),
                        Description = reader.GetString("description"),
                        Price = reader.GetDecimal("price"),
                        CreatedAt = reader.GetDateTime("created_at")
                    };

                    ViewData["product"] = deletedProduct;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }

            return View("Product");

        }
    }
}
