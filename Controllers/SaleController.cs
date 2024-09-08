using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCproyect.Models;
using MVCproyect.Services;
using MySql.Data.MySqlClient;

namespace MVCproyect.Controllers
{
    public class SaleController : Controller
    {

        private readonly MySqlService _context;
        private readonly List<Sale> _sales;
        private readonly IdGeneratorService _idGeneratorService;
        private readonly ProductService _productService;

        public SaleController(MySqlService context)
        {
            _context = context;
            _sales = new List<Sale>();
            _idGeneratorService = new IdGeneratorService(_context);
            _productService = new ProductService(_context);
        }

        // GET: SaleController
        public ActionResult Index()
        {
            try
            {
                using MySqlConnection connection = _context.CreateConnection();

                connection.Open();

                string query = "SELECT * FROM sales";

                using MySqlCommand command = new MySqlCommand(query, connection);

                using MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    Sale sale = new Sale()
                    {
                        Id = reader.GetInt32("Id"),
                        ClientName = reader.GetString("ClientName"),
                        SaleContent = reader.GetString("SaleContent"),
                        TotalSale = reader.GetInt32("TotalSale"),
                        PaymentMethod = reader.GetString("PaymentMethod"),
                        CreatedAt = reader.GetDateTime("CreatedAt"),
                    };

                    _sales.Add(sale);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            ViewData["Sales"] = _sales;

            return View();
        }

        // GET: SaleController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SaleController/Create
        public ActionResult Create()
        {

            List<Product> products = _productService.GetProducts();

            ViewData["products"] = products;

            return View("SaleForm");
        }

        // POST: SaleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SaleController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SaleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SaleController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SaleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
