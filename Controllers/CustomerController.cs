using Microsoft.AspNetCore.Mvc;
using Orders.Dtos;
using Orders.Models;
using Orders.Services;

namespace Orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerService _service;

        public CustomersController(CustomerService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<Customer>> GetAll()
        {
            return Ok(_service.GetAllCustomers());
        }

        [HttpGet("{id}")]
        public ActionResult<Customer> GetById(int id)
        {
            var customer = _service.GetCustomerById(id);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [HttpPost]
        public ActionResult<Customer> Add(Customer customer)
        {
            var created = _service.AddCustomer(customer);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Customer customer)
        {
            var updated = _service.UpdateCustomer(id, customer);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _service.DeleteCustomer(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpGet("NoOrders")]
        public ActionResult<List<Customer>> GetCustomersWithNoOrders()
        {
            var customers = _service.GetCustomersWithNoOrders();
            return Ok(customers);
        }

        [HttpGet("AverageOrders")]
        public ActionResult<List<CustomerAverageDto>> GetCustomerAverageOrderValue()
        {
            var result = _service.GetCustomerAverageOrderValue();
            return Ok(result);
        }

        [HttpGet("LifetimeStats")]
        public ActionResult<List<CustomerLifetimeStatsDto>> GetCustomerLifetimeStats()
        {
            var stats = _service.GetCustomerLifetimeStats();
            return Ok(stats);
        }

        [HttpGet("CustomerOrderAggregates")]
        public ActionResult<List<CustomerOrderAggregateDto>> GetCustomerOrderAggregates()
        {
            var result = _service.GetCustomerOrderAggregates();
            return Ok(result);
        }
    }
}
