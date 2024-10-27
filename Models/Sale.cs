using System;
using System.ComponentModel.DataAnnotations;

namespace MVCproyect.Models
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Client name is required.")]
        [StringLength(100, ErrorMessage = "Client name cannot exceed 100 characters.")]
        public string ClientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Sale content is required.")]
        public string SaleContent { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product sold ID is required.")]
        public int ProductSoldId { get; set; }

        [Required(ErrorMessage = "Total units are required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Total units must be at least 1.")]
        public int TotalUnits { get; set; }

        [Required(ErrorMessage = "Total sale amount is required.")]
        public decimal TotalSale { get; set; }

        [Required(ErrorMessage = "Currency is required")]
        public int Currency { get; set; }

        [Required(ErrorMessage = "Payment method is required.")]
        public string PaymentMethod { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
