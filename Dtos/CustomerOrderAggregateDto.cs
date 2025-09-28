namespace Orders.Dtos
{
    public class CustomerOrderAggregateDto
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
    }
}
