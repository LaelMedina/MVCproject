using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MVCproyect.Models;
using System.Data;
using MVCproyect.Services;
using MVCproyect.Interfaces;
using MVCproyect.Repository;

namespace MVCproyect.Controllers
{
    public class ProductController : Controller
    {
        private readonly MySqlService _context;
        private readonly ProductRepository _productRepository;
        private readonly UserRepository _userRepository;
        private readonly SaleService _saleService;
        private List<Product> _products;
        private List<Currency> _currencies;


        public ProductController(
            MySqlService context, 
            IdGeneratorService idGeneratorService, 
            ProductRepository productRepository,
            UserRepository userRepository,
            SaleService saleService)
        {
            _context = context;
            _products = new List<Product>();
            _productRepository = productRepository;
            _userRepository = userRepository;
            _saleService = saleService;
        }

        public async Task<IActionResult> Index()
        {
            _products = await _productRepository.GetProductsAsync();
            ViewData["Products"] = _products;

            int? loggedUserId = HttpContext.Session.GetInt32("UserId");
            List<Role> rolesList = new List<Role>();

            if (loggedUserId == null)
            {
                return RedirectToAction("Login", "Login");
            }

            User loggedUser = await _userRepository.GetUserByIdAsync(loggedUserId.Value);

            _currencies = await _saleService.GetCurrenciesAsync();

            ViewData["LoggedUser"] = loggedUser;

            ViewData["Currencies"] = _currencies;

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
        public async Task<IActionResult> UpdateProduct(Product updatedProduct)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _productRepository.UpdateProductAsync(updatedProduct);

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
        public async Task<IActionResult> Update(int id)
        {

            try
            {
                Product product = await _productRepository.GetProductByIdAsync(id);

                ViewBag.currentProduct = product;

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("ErrorMessage");
            }

            return View("EditProductForm");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productRepository.DeleteProductAsync(id);
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
