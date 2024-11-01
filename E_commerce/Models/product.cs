using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce.Models
{
    public class product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Rate { get; set; }

        public decimal Price { get; set; }

        public String ImgUrl { get; set; }
        public int categoryId { get; set; }
        public category category { get; set; }

    }
}
