namespace Orders.Dtos
{
    public class CustomerRecentOrderDto
    {
        public string CustomerName { get; set; } = string.Empty;
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
