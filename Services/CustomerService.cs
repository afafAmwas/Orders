using Microsoft.EntityFrameworkCore;
using Orders.DataAccess;
using Orders.Dtos;
using Orders.Models;

namespace Orders.Services
{
    public class CustomerService
    {
        private readonly Repository<Customer> _repo;

        public CustomerService(Repository<Customer> repo)
        {
            _repo = repo;
        }

        public List<Customer> GetAllCustomers()
        {
            return _repo.GetAll().ToList();
        }

        public Customer? GetCustomerById(int id)
        {
            return _repo.GetById(id);
        }

        public Customer AddCustomer(Customer customer)
        {
            _repo.Add(customer);
            _repo.Save();
            return customer;
        }

        public bool UpdateCustomer(int id, Customer customer)
        {
            var existing = _repo.GetById(id);
            if (existing == null)
                return false;

            existing.FullName = customer.FullName;
            _repo.Save();
            return true;
        }

        public bool DeleteCustomer(int id)
        {
            var existing = _repo.GetById(id);
            if (existing == null)
                return false;

            _repo.Delete(existing);
            _repo.Save();
            return true;
        }

        public List<Customer> GetCustomersWithNoOrders()
        {
            return _repo.Query()   // use the Query() method we added earlier
                .Cast<Customer>()
                .Include(c => c.Orders)          
                .Where(c => !c.Orders.Any())     // filter those with no orders
                .ToList();
        }

        public List<CustomerAverageDto> GetCustomerAverageOrderValue()
        {
            return _repo.Query()
                .Cast<Customer>()
                .Include(c => c.Orders)  // load orders
                .Where(c => c.Orders.Count >= 3)
                .Select(c => new CustomerAverageDto
                {
                    FullName = c.FullName,
                    OrderCount = c.Orders.Count,
                    AverageOrderValue = c.Orders.Average(o => o.TotalAmount)
                })
                .ToList();
        }

        public List<CustomerLifetimeStatsDto> GetCustomerLifetimeStats()
        {
            return _repo.Query()
                .Cast<Customer>()
                .Select(c => new CustomerLifetimeStatsDto
                {
                    CustomerId = c.Id,
                    FullName = c.FullName,
                    TotalOrders = c.Orders.Count,
                    TotalSpent = c.Orders.Sum(o => o.TotalAmount),
                    LastOrderDate = c.Orders
                                       .OrderByDescending(o => o.OrderDate)
                                       .Select(o => (DateTime?)o.OrderDate)
                                       .FirstOrDefault()
                })
                .ToList();
        }

        public List<CustomerOrderAggregateDto> GetCustomerOrderAggregates()
        {
            // Use IQueryable to let EF Core generate a single SQL query
            var query = _repo.Query()  // assuming Repository<Customer> exposes IQueryable<Customer> via Query()
                .Select(c => new CustomerOrderAggregateDto
                {
                    CustomerId = c.Id,
                    FullName = c.FullName,
                    TotalOrders = c.Orders.Count(),                 // EF generates COUNT(*)
                    TotalSpent = c.Orders.Sum(o => (decimal?)o.TotalAmount) ?? 0  // EF handles sum with nullable
                })
                .ToList();

            return query;
        }

    }
}
