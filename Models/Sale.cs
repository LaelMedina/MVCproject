using System.ComponentModel.DataAnnotations;

namespace MVCproyect.Models
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ClientName { get; set; } = string.Empty;

        [Required]
        public string SaleContent { get; set; } = string.Empty;

        [Required]
        public int ProductSoldId { get; set; }

        [Required]
        public int TotalUnits { get; set; }

        [Required]
        public decimal TotalSale { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
