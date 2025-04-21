using System.ComponentModel.DataAnnotations;

namespace ElectronicShop.Models
{
    public class ForgotPasswordViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
