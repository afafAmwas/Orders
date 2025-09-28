using Microsoft.AspNetCore.Mvc;
using Orders.Services;
using Orders.Dtos;

namespace Orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _service;

        public OrdersController(OrderService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<OrderReadDto>> GetAll()
        {
            return Ok(_service.GetAllOrders());
        }

        [HttpGet("{id}")]
        public ActionResult<OrderReadDto> GetById(int id)
        {
            var order = _service.GetOrderById(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost]
        public ActionResult<OrderReadDto> Add(OrderCreateUpdateDto dto)
        {
            var created = _service.AddOrder(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, OrderCreateUpdateDto dto)
        {
            var updated = _service.UpdateOrder(id, dto);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _service.DeleteOrder(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpGet("monthly-revenue")]
        public ActionResult<List<MonthlyRevenueDto>> GetMonthlyRevenue()
        {
            var report = _service.GetMonthlyRevenue();
            return Ok(report);
        }

        [HttpGet("top-customers")]
        public ActionResult<List<TopCustomerDto>> GetTopCustomers()
        {
            var topCustomers = _service.GetTopCustomersBySpending();
            return Ok(topCustomers);
        }

        [HttpGet("monthly-profit")]
        public ActionResult<List<MonthlyProfitDto>> GetMonthlyProfit(int year)
        {
            var report = _service.GetMonthlyProfit(year);
            return Ok(report);
        }

        [HttpGet("above-average")]
        public ActionResult<List<OrderReadDto>> GetOrdersAboveCustomerAverage()
        {
            var orders = _service.GetOrdersAboveCustomerAverage();
            return Ok(orders);
        }

        [HttpGet("most-recent-per-customer")]
        public ActionResult<List<CustomerRecentOrderDto>> GetMostRecentOrderPerCustomer()
        {
            var orders = _service.GetMostRecentOrderPerCustomer();
            return Ok(orders);
        }

        [HttpGet("daily-summary")]
        public ActionResult<List<DailySummaryDto>> GetDailyOrderSummary()
        {
            var summary = _service.GetDailyOrderSummary();
            return Ok(summary);
        }


    }
}
