using System.ComponentModel.DataAnnotations.Schema;

namespace MVCPractice.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public OrderHeader OrderHeader { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public int Count { get; set; }
        public decimal Price { get; set; }  // সেই সময়কার দাম সেভ করে রাখা উচিত
    }

}
