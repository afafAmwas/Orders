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
        private readonly CustomerService  _customerService;

        public CustomersController(CustomerService customerService)
        {
             _customerService = customerService;
        }

        [HttpGet]
        public ActionResult<List<Customer>> GetAll()
        {
            return Ok( _customerService.GetAllCustomers());
        }

        [HttpGet("{id}")]
        public ActionResult<Customer> GetById(int id)
        {
            var customer =  _customerService.GetCustomerById(id);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [HttpPost]
        public ActionResult<Customer> Add(Customer customer)
        {
            var created =  _customerService.AddCustomer(customer);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Customer customer)
        {
            var updated =  _customerService.UpdateCustomer(id, customer);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted =  _customerService.DeleteCustomer(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpGet("NoOrders")]
        public ActionResult<List<Customer>> GetCustomersWithNoOrders()
        {
            var customers =  _customerService.GetCustomersWithNoOrders();
            return Ok(customers);
        }

        [HttpGet("AverageOrders")]
        public ActionResult<List<CustomerAverageDto>> GetCustomerAverageOrderValue()
        {
            var result =  _customerService.GetCustomerAverageOrderValue();
            return Ok(result);
        }

        [HttpGet("LifetimeStats")]
        public ActionResult<List<CustomerLifetimeStatsDto>> GetCustomerLifetimeStats()
        {
            var stats =  _customerService.GetCustomerLifetimeStats();
            return Ok(stats);
        }

        [HttpGet("CustomerOrderAggregates")]
        public ActionResult<List<CustomerOrderAggregateDto>> GetCustomerOrderAggregates()
        {
            var result =  _customerService.GetCustomerOrderAggregates();
            return Ok(result);
        }
    }
}
