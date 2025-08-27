using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCPractice.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser ApplicationUser { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FullName { get; set; }
        public string? City { get;set; }


        public DateTime OrderDate { get; set; }
        public decimal OrderTotal { get; set; }

        public string OrderStatus { get; set; }   // Pending, Approved, Shipped, Delivered
        public string PaymentStatus { get; set; } // Pending, Paid, Refunded

        public string? ShippingAddress { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }

}
