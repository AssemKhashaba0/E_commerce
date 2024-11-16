using E_commerce.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Order
{
    [Key]
    public int OrderId { get; set; }  // Primary Key

    [ForeignKey("ApplicationUser")]
    public string ApplicationUserId { get; set; }  // Foreign Key to ApplicationUser

    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; }

    // Navigation property
    public ApplicationUser ApplicationUser { get; set; }

    // إضافة العلاقة مع OrderDetail
    public ICollection<OrderDetail> OrderDetails { get; set; }  // يمثل تفاصيل الطلب

    public ICollection<OrderTracking> OrderTrackingDetails { get; set; }
}
