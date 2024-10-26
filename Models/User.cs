using System.ComponentModel.DataAnnotations;

namespace MVCproyect.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "The user name is required")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The user password is required")]
        public string PasswordHash { get; set; } = string.Empty;

        [Required(ErrorMessage = "The user role is required")]
        public int RoleId { get; set; }

    }
}
