using System.ComponentModel.DataAnnotations;

namespace MVCproyect.Models
{
    public class Sale
    {
        public int Id { get; set; }

        public string ClientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "A product must be selected")]
        public string SaleContent { get; set; } = string.Empty;

        [Required(ErrorMessage = "The total units must be inserted")]
        public int TotalUnits { get; set; }

        public decimal TotalSale { get; set; }

        [Required(ErrorMessage = "The payment Method must be selected")]
        public string PaymentMethod { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
