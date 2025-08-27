using System.ComponentModel.DataAnnotations.Schema;

namespace MVCPractice.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

    }
}
