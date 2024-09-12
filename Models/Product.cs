using MathNet.Numerics;
using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;

namespace MVCproyect.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The name of the product is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Product's description is required")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Product's price is required")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "The amount of units of the product must be specified")]
        public int Stock { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
