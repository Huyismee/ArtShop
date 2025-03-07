using System.ComponentModel.DataAnnotations;

namespace ArtShopAPI.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? password { get; set; }
    }
}
