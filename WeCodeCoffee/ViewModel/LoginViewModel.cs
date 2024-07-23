using System.ComponentModel.DataAnnotations;

namespace WeCodeCoffee.ViewModel
{
    public class LoginViewModel
    {
      
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email address is required")]
        public string EmailAddress{ get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
