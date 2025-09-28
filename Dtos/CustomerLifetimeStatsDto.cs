namespace Orders.Dtos
{
    public class CustomerLifetimeStatsDto
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public DateTime? LastOrderDate { get; set; }
    }
}
