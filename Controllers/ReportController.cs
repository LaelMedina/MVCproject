using SpreadsheetLight;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVCproyect.Interfaces;
using MVCproyect.Models;
using MVCproyect.Repository;

public class ReportController : Controller
{
    private readonly ProductRepository _productRepository;
    private readonly SaleRepository _saleRepository;

    public ReportController(ProductRepository productRepository, SaleRepository saleRepository)
    {
        _productRepository = productRepository;
        _saleRepository = saleRepository;
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
        List<Sale> sales = await _saleRepository.GetSalesAsync();
        var memoryStream = new MemoryStream();

        SLDocument sLDocument = new SLDocument();
        System.Data.DataTable dt = new System.Data.DataTable();

        // Excel Columns
        dt.Columns.Add("Id", typeof(int));
        dt.Columns.Add("Client", typeof(string));
        dt.Columns.Add("Product", typeof(string));
        dt.Columns.Add("Units", typeof(int));
        dt.Columns.Add("Total", typeof(decimal));
        dt.Columns.Add("Payment Method", typeof(string));
        dt.Columns.Add("Date", typeof(string));

        // Excel Data Rows
        foreach (Sale sale in sales)
        {
            dt.Rows.Add(sale.Id, sale.ClientName, sale.SaleContent, sale.TotalUnits, sale.TotalSale, sale.PaymentMethod, sale.CreatedAt);
        }

        sLDocument.ImportDataTable(1, 1, dt, true);
        sLDocument.SaveAs(memoryStream);

        // Rewind the stream position to the beginning before returning it
        memoryStream.Position = 0;

        string fileName = $"SalesReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

        return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }
}
