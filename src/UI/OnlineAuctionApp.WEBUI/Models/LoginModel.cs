using System.ComponentModel.DataAnnotations;

namespace OnlineAuctionApp.WEBUI.Models
{
    public class LoginModel
    {
        [EmailAddress]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email field is required")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "Password min 4 must be character")]
        [Required(ErrorMessage = "Password field is required")]
        public string Password { get; set; }
    }
}
