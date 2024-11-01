using MathNet.Numerics;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Fields;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MVCproyect.Interfaces;
using MVCproyect.Models;
using MySql.Data.MySqlClient;
using PdfSharp.Pdf;

namespace MVCproyect.Services
{
    public class SaleService : ISaleService
    {

        private readonly MySqlService _context;

        public SaleService(MySqlService context)
        {
            _context = context;
        }

        public async Task<List<Currency>> GetCurrenciesAsync()
        {
            List<Currency> _currencies = new List<Currency>();

            try
            {
                using MySqlConnection connection = _context.CreateConnection();

                await connection.OpenAsync();

                string query = "SELECT * FROM tb_currency";

                using MySqlCommand command = new MySqlCommand(query, connection);

                using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    Currency currency = new Currency()
                    {
                        Id = reader.GetInt32("id"),
                        CurrencyName = reader.GetString("currency"),
                        Acronym = reader.GetString("acronym"),
                    };

                    _currencies.Add(currency);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return _currencies;
        }

        public async Task<List<PaymentMethod>> GetPaymentMethodsAsync()
        {
            List<PaymentMethod> _paymentMethods = new List<PaymentMethod>();

            try
            {
                using MySqlConnection connection = _context.CreateConnection();

                await connection.OpenAsync();

                string query = "SELECT * FROM paymentmethods";

                using MySqlCommand command = new MySqlCommand(query, connection);

                using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    PaymentMethod paymentMethod = new PaymentMethod()
                    {
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("name"),
                    };

                    _paymentMethods.Add(paymentMethod);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return _paymentMethods;
        }

        public PdfDocument GenerateInvoiceSale(Sale newSale)
        {
            var document = new Document();

            BuildDocument(document, newSale);

            var pdfRenderer = new PdfDocumentRenderer();
            pdfRenderer.Document = document;

            pdfRenderer.RenderDocument();

            return pdfRenderer.PdfDocument;
        }

        private void BuildDocument(Document document, Sale newSale)
        {
            Section section = document.AddSection();

            //Header
            var paragraph = section.AddParagraph();
            paragraph.AddText("BEST STORE");
            paragraph.AddLineBreak();
            paragraph.AddText("website: www.beststore.com");
            paragraph.AddLineBreak();
            paragraph.AddText("Email: beststore@gmail.com");
            paragraph.AddLineBreak();
            paragraph.AddText("Phone: 8660-1995");
            paragraph.Format.SpaceAfter = 20;

            //Customer Details
            paragraph = section.AddParagraph();
            paragraph.AddText("Invoice No. " + newSale.Id); //Id
            paragraph.AddLineBreak();
            paragraph.AddText("Bailed To. " + newSale.ClientName); //Client Name
            paragraph.AddLineBreak();
            paragraph.AddText("Payment Method. " + newSale.PaymentMethod); //Payment Method
            paragraph.AddLineBreak();
            paragraph.Add(new DateField { Format = "yyyy/MM/dd HH:mm:ss" });
            paragraph.Format.SpaceAfter = 10;

            var table = document.LastSection.AddTable();
            table.Borders.Width = 0.5;

            //Table columns
            table.AddColumn("1cm");
            table.AddColumn("9cm");
            table.AddColumn("3cm");
            table.AddColumn("3cm");

            //Table headers
            Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Font.Bold = true;
            row.Cells[0].AddParagraph("Id");
            row.Cells[1].AddParagraph("Product");
            row.Cells[2].AddParagraph("Price");
            row.Cells[3].AddParagraph("Units");

            // Adding product rows
            for (int i = 0; i < newSale.SaleDetails.Count; i++)
            {
                row = table.AddRow();
                row.Cells[0].AddParagraph(newSale.SaleDetails[i].ProductId.ToString());
                row.Cells[1].AddParagraph(newSale.SaleDetails[i].ProductName);
                row.Cells[2].AddParagraph(newSale.SaleDetails[i].Price.ToString());
                row.Cells[3].AddParagraph(newSale.SaleDetails[i].Units.ToString());
            }

            decimal Iva = 0.15m;
            decimal total = Math.Round(newSale.TotalSale * Iva, 2);

            var tableTotal = section.AddTable();
            tableTotal.Borders.Width = 0;

            var column1 = tableTotal.AddColumn("5cm");
            var column2 = tableTotal.AddColumn("3cm"); 

            var rowTotal = tableTotal.AddRow();
            rowTotal.Cells[0].AddParagraph("Sub Total:");
            rowTotal.Cells[1].AddParagraph($"{newSale.TotalSale:F2} USD");

            rowTotal = tableTotal.AddRow();
            rowTotal.Cells[0].AddParagraph("IVA:");
            rowTotal.Cells[1].AddParagraph($"{(newSale.TotalSale * Iva):F2} USD");

            rowTotal = tableTotal.AddRow();
            rowTotal.Cells[0].AddParagraph("Total:");
            rowTotal.Cells[1].AddParagraph($"{(newSale.TotalSale + total):F2} USD");

            paragraph = section.AddParagraph();
            paragraph.Format.SpaceAfter = 10;

            paragraph = section.Footers.Primary.AddParagraph();
            paragraph.AddText("Best Store Inc. Leon, Nicaragua.");
            paragraph.Format.Alignment = ParagraphAlignment.Center;
        }
    }
}
