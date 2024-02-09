﻿using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int QuANTITY { get; set; }

        [Precision(16,2)]
        public decimal UnitPrice { get; set; }
        //navigation properties 
        public Order Order { get; set; } = null!;
        public Product Product { get; set; } = null!;


    }
}
