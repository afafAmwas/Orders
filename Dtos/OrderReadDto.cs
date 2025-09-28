namespace Orders.Dtos
{
    public class OrderReadDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal CostAmount { get; set; }

    }

}

