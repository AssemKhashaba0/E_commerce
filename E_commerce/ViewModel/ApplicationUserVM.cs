using System.ComponentModel.DataAnnotations;

namespace E_commerce.ViewModel
{
    public class ApplicationUserVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter your name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your email address.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Make sure to enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please create a password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "The passwords you entered do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please enter your phone number.")]
        [StringLength(17, ErrorMessage = "Your phone number can be up to 15 characters long.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please enter your city.")]
        public string City { get; set; }

    }
}
