using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class OrderTracking
{
    [Key]
    public int TrackingId { get; set; }  // Primary Key

    [ForeignKey("Order")]
    public int OrderId { get; set; }  // Foreign Key to Order

    public string Status { get; set; }  // e.g., "Processing", "Shipped", "Delivered"
    public DateTime UpdatedAt { get; set; }  // Timestamp for the update
    public string Notes { get; set; }  // Optional additional information

    // Navigation property
    public  Order Order { get; set; }
}
