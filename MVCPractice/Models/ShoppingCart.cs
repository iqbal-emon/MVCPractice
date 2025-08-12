﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCPractice.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser ApplcationUsers { get; set; }

    }
}
