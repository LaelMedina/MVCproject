using Microsoft.AspNetCore.Mvc;
using MVCproyect.Models;

namespace MVCproyect.Controllers
{
    public class ProductController : Controller
    {
        private readonly List<Product> _products = new List<Product>
        {
            new Product
            {
                Name = "Anti Reflection Glasses",
                Description = "Built with the exact material you need to avoid blue light dangerous effects on your eyes",
                Price = 99.9
            },

            new Product
            {
                Name = "Mic Setup",
                Description = "A mic setup with all the tools you need to start a podcast",
                Price = 199.9
            },

            new Product 
            {
                Name = "Camera Setup",
                Description = "All the visual tools you need to start a podcast",
                Price = 499.9
            }
        };


        public IActionResult Index() 
        {
            ViewData["Products"] = _products;
            return View();
        }

        public void Create() { }

        public void Read() { }

        public void Update() { }

        public void Delete(){ }
    }
}
