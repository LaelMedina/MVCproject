using System.ComponentModel.DataAnnotations;

namespace MVCproyect.Models
{
    public class Seller
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The user name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "The user Identity is required")]
        public string Identity { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
