using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MVCproyect.Models;
using MVCproyect.Repository;
using MVCproyect.Services;
using MySql.Data.MySqlClient;
using System.ComponentModel;

namespace MVCproyect.Controllers
{
    public class SaleController : Controller
    {

        private readonly MySqlService _context;
        private List<Sale> _sales;
        private readonly IdGeneratorService _idGeneratorService;
        private readonly ProductService _productService;
        private readonly SaleService _saleService;
        private readonly SaleRepository _saleRepository;

        public SaleController(MySqlService context, SaleRepository saleRepository)
        {
            _context = context;
            _sales = new List<Sale>();
            _idGeneratorService = new IdGeneratorService(_context);
            _productService = new ProductService(_context);
            _saleService = new SaleService(_context);
            _saleRepository = saleRepository;
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                _sales = await _saleRepository.GetSalesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            ViewData["Sales"] = _sales;

            return View();
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public async Task<ActionResult> Create()
        {

            List<Product> products = await _productService.GetProductsAsync();

            List<PaymentMethod> paymentMethods = await _saleService.GetPaymentMethodsAsync();

            ViewData["products"] = products;

            ViewData["paymentMethods"] = paymentMethods;

            return View("SaleForm");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Sale newSale)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    await _saleRepository.AddSaleAsync(newSale);

                    _productService.UpdateStockAfterSale(newSale.ProductSoldId, newSale.TotalUnits);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

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

        public ActionResult Delete(int id)
        {
            return View();
        }

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
