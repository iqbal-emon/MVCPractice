using System.ComponentModel;

namespace MVCPractice.Models
{
    public class Category
    {
        public int Id { get; set; }
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Order Id")]
        public int OrderId { get; set; }
    }
}
