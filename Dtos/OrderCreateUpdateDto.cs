namespace Orders.Dtos
{
    public class OrderCreateUpdateDto
    {
        public int CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
    }
}



