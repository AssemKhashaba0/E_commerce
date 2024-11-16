using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce.Models
{
    [PrimaryKey("productId", "ApplicationUsertId")]
    public class Cart
    {
        public  int productId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(productId))]
        public product product { get; set; }

        public string  ApplicationUsertId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(ApplicationUsertId))]
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        [Range(1,1000)]
        public int Count { get; set; }
    }
}
