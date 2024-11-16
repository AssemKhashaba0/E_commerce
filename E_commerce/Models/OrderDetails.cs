using E_commerce.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce.Models
{


public class OrderDetail
{
    [Key]
    public int OrderDetailId { get; set; }  // Primary Key

    [Required]
    [ForeignKey("Order")]
    public int OrderId { get; set; }  // Foreign Key to Order

    [Required]
    [ForeignKey("product")]
    public int productId { get; set; }  // Foreign Key to Product

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }
  public decimal Total { get; set; }  // المجموع الكلي للسعر * الكمية

        // Navigation properties
   public Order Order { get; set; }
    public product product { get; set; }
}
}