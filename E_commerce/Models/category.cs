namespace E_commerce.Models
{
    public class category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<product> Products { get; }=new List<product>();
        
    }
}
