namespace Orders.Dtos
{
    public class DailySummaryDto
    {
        public DateTime OrderDate { get; set; }
        public int OrderCount { get; set; }
        public decimal DailyRevenue { get; set; }
    }
}
