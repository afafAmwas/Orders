namespace Orders.Dtos
{
    public class CustomerAverageDto
    {
        public string FullName { get; set; } = string.Empty;
        public int OrderCount { get; set; }
        public decimal AverageOrderValue { get; set; }
    }
}
