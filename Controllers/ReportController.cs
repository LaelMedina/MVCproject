using SpreadsheetLight;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVCproyect.Interfaces;
using MVCproyect.Models;
using MVCproyect.Repository;
using MVCproyect.Services;

public class ReportController : Controller
{
    private readonly ProductRepository _productRepository;
    private readonly SaleRepository _saleRepository;
    private readonly SaleService _saleService;

    public ReportController(ProductRepository productRepository, SaleRepository saleRepository, SaleService saleService)
    {
        _productRepository = productRepository;
        _saleRepository = saleRepository;
        _saleService = saleService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GenerateProductsStockReportExcel()
    {
        List<Product> products = await _productRepository.GetProductsAsync();
        var memoryStream = new MemoryStream();

        SLDocument sLDocument = new SLDocument();
        System.Data.DataTable dt = new System.Data.DataTable();

        // Excel Columns
        dt.Columns.Add("Id", typeof(int));
        dt.Columns.Add("Name", typeof(string));
        dt.Columns.Add("Price", typeof(decimal));
        dt.Columns.Add("Stock", typeof(int));

        // Excel Data Rows
        foreach (Product product in products)
        {
            dt.Rows.Add(product.Id, product.Name, product.Price, product.Stock);
        }

        sLDocument.ImportDataTable(1, 1, dt, true);
        sLDocument.SaveAs(memoryStream);

        // Rewind the stream position to the beginning before returning it
        memoryStream.Position = 0;

        string fileName = $"ProductsStockReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

        return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
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
