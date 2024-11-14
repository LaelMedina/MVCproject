using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MVCproyect.Interfaces;
using MVCproyect.Models;
using MVCproyect.Repository;
using MVCproyect.Services;
using MySql.Data.MySqlClient;
using System.ComponentModel;
using Newtonsoft.Json;
using SpreadsheetLight;

namespace MVCproyect.Controllers
{
    public class SaleController : Controller
    {

        private readonly MySqlService _context;
        private readonly IdGeneratorService _idGeneratorService;
        private readonly ProductService _productService;
        private readonly SaleService _saleService;
        private readonly SaleRepository _saleRepository;
        private readonly UserRepository _userRepository;
        private readonly SellerRepository _sellerRepository;
        private List<Sale> _sales;
        private List<Currency> _currencies;
        private List<Seller> _sellers = new();

        public SaleController(MySqlService context, SaleRepository saleRepository, UserRepository userRepository, SellerRepository sellerRepository)
        {
            _context = context;
            _idGeneratorService = new IdGeneratorService(_context);
            _productService = new ProductService(_context);
            _saleService = new SaleService(_context);
            _saleRepository = saleRepository;
            _userRepository = userRepository;
            _sales = new List<Sale>();
            _currencies = new List<Currency>();
            _sellerRepository = sellerRepository;
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                int? loggedUserId = HttpContext.Session.GetInt32("UserId");
                List<Role> rolesList = new List<Role>();

                if (loggedUserId == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                User loggedUser = await _userRepository.GetUserByIdAsync(loggedUserId.Value);

                ViewData["LoggedUser"] = loggedUser;

                _sales = await _saleRepository.GetSalesAsync();
                _currencies = await _saleService.GetCurrenciesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            ViewData["Sales"] = _sales;
            ViewData["Currencies"] = _currencies;

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

            _currencies = await _saleService.GetCurrenciesAsync();

            _sellers = await _sellerRepository.GetSellersAsync();

            ViewData["Sellers"] = _sellers;

            ViewData["products"] = products;

            ViewData["paymentMethods"] = paymentMethods;

            ViewData["Currencies"] = _currencies;


            return View("SaleForm");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Sale newSale,[FromForm] string CartJson)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var saleDetails = JsonConvert.DeserializeObject<List<SaleDetail>>(CartJson);
                    newSale.SaleDetails = saleDetails;

                    await _saleRepository.AddSaleAsync(newSale);

                    foreach (var item in saleDetails)
                    {
                        await _productService.UpdateStockAfterSale(item.ProductId, item.Units);
                    }

                    var document = _saleService.GenerateInvoiceSale(newSale);

                    MemoryStream stream = new MemoryStream();
                    document.Save(stream);
                    string fileName = "Invoice_" + newSale.Id + ".pdf";
                    Response.ContentType = "application/pdf";
                    Response.Headers.Append("content-length", stream.Length.ToString());
                    byte[] bytes = stream.ToArray();
                    stream.Close();

                    return File(bytes, "appliccation/pdf", fileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                    return View("ErrorMessage", new { message = ex.Message });
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

                _currencies = await _saleService.GetCurrenciesAsync();

                ViewData["products"] = products;

                ViewData["paymentMethods"] = paymentMethods;

                ViewData["Currencies"] = _currencies;

                ViewData["Sellers"] = await _sellerRepository.GetSellersAsync();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("ErrorMessage");
            }

            return View("EditSaleForm");
        }

        [HttpPost]
        public async Task<ActionResult> UpdateSale(Sale updatedSale, [FromForm] string CartJson)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var saleDetails = JsonConvert.DeserializeObject<List<SaleDetail>>(CartJson);

                    updatedSale.SaleDetails = saleDetails;

                    await _saleRepository.UpdateSaleAsync(updatedSale);

                    foreach (var item in saleDetails)
                    {
                        await _productService.UpdateStockAfterSale(item.ProductId, item.Units);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "An error has occurred: " + ex.Message + " Sale Id: " + updatedSale.Id;
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

        [HttpGet]
        public async Task<IActionResult> GenerateSalesReportExcel()
        {
            List<Sale> sales = await _saleRepository.GetSalesWithDetailsAsync();
            List<Currency> currencies = await _saleService.GetCurrenciesAsync();
            var memoryStream = new MemoryStream();

            SLDocument sLDocument = new SLDocument();
            System.Data.DataTable dt = new System.Data.DataTable();

            dt.Columns.Add("Sale Id", typeof(int));
            dt.Columns.Add("Client", typeof(string));
            dt.Columns.Add("Seller", typeof(string));
            dt.Columns.Add("Total Sale", typeof(decimal));
            dt.Columns.Add("Currency", typeof(string));
            dt.Columns.Add("Payment Method", typeof(string));
            dt.Columns.Add("Sale Date", typeof(string));
            dt.Columns.Add("Product Name", typeof(string));
            dt.Columns.Add("Units", typeof(int));
            dt.Columns.Add("Price", typeof(decimal));
            dt.Columns.Add("Total Price (Dollars)", typeof(decimal));

            foreach (Sale sale in sales)
            {
                foreach (var detail in sale.SaleDetails)
                {
                    dt.Rows.Add(
                        sale.Id,
                        sale.ClientName,
                        sale.SellerName,
                        sale.TotalSale,
                        sale.Currency,
                        sale.PaymentMethod,
                        sale.CreatedAt,
                        detail.ProductName,
                        detail.Units,
                        detail.Price,
                        sale.TotalSale
                    );
                }
            }

            sLDocument.ImportDataTable(1, 1, dt, true);
            sLDocument.SaveAs(memoryStream);

            memoryStream.Position = 0;

            string fileName = $"SalesReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
