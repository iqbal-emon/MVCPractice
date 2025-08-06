using System.ComponentModel.DataAnnotations.Schema;

namespace MVCPractice.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

    }
}
