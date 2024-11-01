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

        [Required(ErrorMessage = "Total sale amount is required.")]
        public decimal TotalSale { get; set; }

        [Required(ErrorMessage = "Currency is required")]
        public string Currency { get; set; } = string.Empty;

        [Required(ErrorMessage = "Payment method is required.")]
        public string PaymentMethod { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
    }
}
