namespace MVCproyect.Models
{
    public class SellerReport
    {
        public int SellerId { get; set; }
        public string SellerName { get; set; } = string.Empty;
        public int TotalSales { get; set; }
        public int TotalUnits { get; set; }
        public decimal TotalIncome { get; set; }
    }
}
