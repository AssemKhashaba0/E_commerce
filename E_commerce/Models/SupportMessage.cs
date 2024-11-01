using System.ComponentModel.DataAnnotations;

namespace E_commerce.Models
{
    public class SupportMessage
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string UserEmail { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
