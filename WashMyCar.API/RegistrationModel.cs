using System.ComponentModel.DataAnnotations;

namespace WashMyCar.API.Models
{
    public class RegistrationModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required, MinLength(8), DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Cellphone { get; set; }
        public string Address { get; set; }
    }
}