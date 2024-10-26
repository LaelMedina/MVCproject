using System;
using System.ComponentModel.DataAnnotations;

namespace MVCproyect.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The name of the product is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "The product's description is required.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "The product's price is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "The price must be at least 1.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "The amount of units of the product must be specified.")]
        [Range(1, int.MaxValue, ErrorMessage = "Total units must be at least 1.")]
        public int Stock { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
