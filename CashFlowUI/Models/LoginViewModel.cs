using System.ComponentModel.DataAnnotations;

namespace CashFlowUI.Models
{
    public class LoginViewModel
    {
        [Display(Name = "User")]
        [Required]
        public string User { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
