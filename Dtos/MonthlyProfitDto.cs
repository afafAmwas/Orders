namespace Orders.Dtos
{
    public class MonthlyProfitDto
    {
        public int Month { get; set; }
        public decimal Revenue { get; set; }
        public decimal Cost { get; set; }
        public decimal Profit { get; set; }
    }
}
