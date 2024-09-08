namespace MVCproyect.Models
{
    public class Sale
    {
        public int Id { get; set; }

        public string ClientName { get; set; } = string.Empty;

        public decimal TotalSale { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
