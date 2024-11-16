using E_commerce.Migrations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce.Models
{
    public class product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "It must not be empty")]
        [MinLength(3, ErrorMessage = "It must be at least 3 characters")]
        [MaxLength(50, ErrorMessage = "It must not exceed 50 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "It must not be empty")]
        [MinLength(3, ErrorMessage = "It must be at least 3 characters")]
        public string Description { get; set; }
        [Range(0, 5 , ErrorMessage = "The value must be between 0 and 5.")]

        public decimal Rate { get; set; }
        [Required(ErrorMessage = "It must not be empty")]
        [Range(0,100000, ErrorMessage = "The value must be between 0 and 100000.")]
        public decimal Price { get; set; }
        [ValidateNever]
        public String ImgUrl { get; set; }
        [Required(ErrorMessage = "It must not be empty")]
        public int categoryId { get; set; }
        public int? campanyId { get; set; }

        [ValidateNever]
        public category category { get; set; }
        [ValidateNever]
        public Campany campany { get; set; }

        public List<Cart> carts { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
        public List<OrderTracking> OrderTrackings { get; set; }

    }
}

