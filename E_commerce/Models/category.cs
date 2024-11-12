using System.ComponentModel.DataAnnotations;

namespace E_commerce.Models
{
    public class category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "It must not be empty")]
        [MinLength(3 ,ErrorMessage = "It must be at least 3 characters")]
        [MaxLength(50,ErrorMessage = "It must not exceed 50 characters")]
        public string Name { get; set; }

        public ICollection<product> Products { get; }=new List<product>();
        
    }
}
