using System.ComponentModel.DataAnnotations;

namespace MVCproyect.Models
{
    public class Sale
    {
        public int Id { get; set; }

        public string ClientName { get; set; } = string.Empty;

        public string SaleContent { get; set; } = string.Empty;

        public int TotalUnits { get; set; }

        public decimal TotalSale { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
