namespace MVCproyect.Models
{
    public class SaleDetail
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Units { get; set; }
    }

}
