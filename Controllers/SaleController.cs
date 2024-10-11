using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MVCproyect.Interfaces;
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

                    await _productService.UpdateStockAfterSale(newSale.ProductSoldId, newSale.TotalUnits);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                Sale sale = await _saleRepository.GetSaleByIdAsync(id);

                ViewBag.currentSale = sale;

                List<Product> products = await _productService.GetProductsAsync();

                List<PaymentMethod> paymentMethods = await _saleService.GetPaymentMethodsAsync();

                ViewData["products"] = products;

                ViewData["paymentMethods"] = paymentMethods;

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("ErrorMessage");
            }

            return View("EditSaleForm");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateSale(Sale updatedSale)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _saleRepository.UpdateSaleAsync(updatedSale);

                    ViewData["Sales"] = updatedSale;
                }
                catch(Exception ex)
                {
                    ViewBag.ErrorMessage = "An error has occured: " + ex.Message.ToString() + "Sale Id: " + updatedSale.Id;
                    return View("ErrorView");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _saleRepository.DeleteSaleAsync(id);
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
