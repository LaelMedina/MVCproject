using System.ComponentModel.DataAnnotations;

namespace MVCproyect.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Product's name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Product's description is required")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Product's price is required")]
        public double Price { get; set; }

    }
}
