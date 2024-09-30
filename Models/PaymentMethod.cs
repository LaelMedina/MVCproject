using System.ComponentModel.DataAnnotations;

namespace MVCproyect.Models
{
    public class PaymentMethod
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

    }
}
