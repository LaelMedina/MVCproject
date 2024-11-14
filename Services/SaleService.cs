using MathNet.Numerics;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Fields;
using MigraDoc.DocumentObjectModel.Shapes;
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
            document.Info.Title = "Invoice - Best Store";

            BuildDocument(document, newSale);

            var pdfRenderer = new PdfDocumentRenderer();
            pdfRenderer.Document = document;
            pdfRenderer.RenderDocument();

            return pdfRenderer.PdfDocument;
        }

        private void BuildDocument(Document document, Sale newSale)
        {
            Section section = document.AddSection();

            // Encabezado
            Image logo = section.AddImage("Images/beststore_logo.jpg");
            logo.Width = "2cm";
            logo.LockAspectRatio = true;
            logo.Left = ShapePosition.Left;
            logo.Top = ShapePosition.Top;

            var paragraph = section.AddParagraph();
            paragraph.AddFormattedText("BEST STORE", TextFormat.Bold);
            paragraph.Format.Font.Size = 18;
            paragraph.Format.Font.Color = Colors.DarkBlue;
            paragraph.Format.SpaceAfter = 3;

            paragraph = section.AddParagraph();
            paragraph.AddText("Website: www.beststore.com");
            paragraph.AddLineBreak();
            paragraph.AddText("Email: beststore@gmail.com");
            paragraph.AddLineBreak();
            paragraph.AddText("Phone: 8660-1995");
            paragraph.Format.SpaceAfter = 20;

            // Detalles del cliente
            paragraph = section.AddParagraph();
            paragraph.AddFormattedText("Invoice No. " + newSale.Id, TextFormat.Bold);
            paragraph.Format.Font.Size = 10;
            paragraph.AddLineBreak();
            paragraph.AddFormattedText("Billed To: " + newSale.ClientName, TextFormat.Italic);
            paragraph.AddLineBreak();
            paragraph.AddFormattedText("Billed By: " + newSale.SellerName, TextFormat.Italic);
            paragraph.AddLineBreak();
            paragraph.AddText("Payment Method: " + newSale.PaymentMethod);
            paragraph.AddLineBreak();
            paragraph.AddText("Date: ");
            paragraph.Add(new DateField { Format = "yyyy/MM/dd HH:mm:ss" });
            paragraph.Format.SpaceAfter = 15;

            // Tabla de productos
            var table = section.AddTable();
            table.Borders.Width = 0.5;
            table.Borders.Color = Colors.Gray;
            table.LeftPadding = 5;
            table.RightPadding = 5;
            table.Rows.LeftIndent = 5;

            table.AddColumn("1.5cm");
            table.AddColumn("6cm");
            table.AddColumn("3cm").Format.Alignment = ParagraphAlignment.Right;
            table.AddColumn("2.5cm").Format.Alignment = ParagraphAlignment.Right;

            // Encabezados de tabla
            Row row = table.AddRow();
            row.Shading.Color = Colors.LightGray;
            row.HeadingFormat = true;
            row.Format.Font.Bold = true;
            row.Cells[0].AddParagraph("ID");
            row.Cells[1].AddParagraph("Product");
            row.Cells[2].AddParagraph("Price (USD)");
            row.Cells[3].AddParagraph("Units");

            // Agregar productos al carrito
            foreach (var item in newSale.SaleDetails)
            {
                row = table.AddRow();
                row.Cells[0].AddParagraph(item.ProductId.ToString());
                row.Cells[1].AddParagraph(item.ProductName);
                row.Cells[2].AddParagraph($"{item.Price:F2}");
                row.Cells[3].AddParagraph(item.Units.ToString());
            }

            // Sección de Totales
            var totalTable = section.AddTable();
            totalTable.Borders.Width = 0;
            totalTable.AddColumn("6cm");
            totalTable.AddColumn("3cm").Format.Alignment = ParagraphAlignment.Right;

            decimal ivaRate = 0.15m;
            decimal subTotal = newSale.TotalSale;
            decimal ivaAmount = Math.Round(subTotal * ivaRate, 2);
            decimal grandTotal = subTotal + ivaAmount;

            Row totalRow = totalTable.AddRow();
            totalRow.Cells[0].AddParagraph("Sub Total:");
            totalRow.Cells[1].AddParagraph($"{subTotal:F2} USD");

            totalRow = totalTable.AddRow();
            totalRow.Cells[0].AddParagraph("IVA:");
            totalRow.Cells[1].AddParagraph($"{ivaAmount:F2} USD");

            totalRow = totalTable.AddRow();
            totalRow.Cells[0].AddParagraph("Total:");
            totalRow.Cells[1].AddParagraph($"{grandTotal:F2} USD");

            paragraph = section.AddParagraph();
            paragraph.Format.SpaceAfter = 10;

            // Pie de página
            paragraph = section.Footers.Primary.AddParagraph();
            paragraph.AddText("Thank you for your business with Best Store Inc.");
            paragraph.AddLineBreak();
            paragraph.AddText("Leon, Nicaragua.");
            paragraph.Format.Alignment = ParagraphAlignment.Center;
        }

    }
}
