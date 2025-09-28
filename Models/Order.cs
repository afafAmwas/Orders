﻿namespace Orders.Models
{
    public class Order
    {
        public int Id { get; set; }
        
        public int CustomerId { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime OrderDate { get; set; }
        public Customer Customer { get; set; } = null!;

        public decimal CostAmount { get; set; }
    }
}
