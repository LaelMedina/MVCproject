﻿using Microsoft.AspNetCore.Mvc;
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

                Console.WriteLine("Before the while loop");

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
                    Console.WriteLine($"Added product: {product.Name}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

            ViewData["Products"] = _products;
            return View();
        }

        public void Create() { }

        public void Read() { }

        public void Update() { }

        public void Delete() { }
    }
}
