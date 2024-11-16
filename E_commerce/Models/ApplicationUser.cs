using Microsoft.AspNetCore.Identity;

namespace E_commerce.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string City { get; set; }
        public List<Cart> carts { get; set; }

    }
}
