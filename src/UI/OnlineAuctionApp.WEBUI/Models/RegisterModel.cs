using OnlineAuctionApp.WEBUI.Enums;
using System.ComponentModel.DataAnnotations;

namespace OnlineAuctionApp.WEBUI.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "UserName field is required")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "First Name field is required")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Last Name field is required")]
        [Display(Name = "LastName")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Phone Number field is required")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email field is required")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password field is required")]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [Required(ErrorMessage = "IsBuyer field is required")]
        [Display(Name = "IsBuyer")]
        public bool IsBuyer { get; set; }//alıcı mı


        [Required(ErrorMessage = "IsSeller field is required")]
        [Display(Name = "IsSeller")]
        public bool IsSeller { get; set; }//tedarikci/satıcı mı

        public UserType UserSelectType { get; set; }
    }
}
