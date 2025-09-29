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

        ///////////////////////////////////////////////////////////////////////- tasks start here

        public List<Customer> GetCustomersWithNoOrders()
        {
            return _repo.QueryWithInclude(c => c.Orders)
                .Where(c => !c.Orders.Any())
                .ToList();
        }

        public List<CustomerAverageDto> GetCustomerAverageOrderValue()
        {
            return _repo.QueryWithInclude(c => c.Orders)
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
            return _repo.QueryWithInclude(c => c.Orders)
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
            return _repo.QueryWithInclude(c => c.Orders)
                .Select(c => new CustomerOrderAggregateDto
                {
                    CustomerId = c.Id,
                    FullName = c.FullName,
                    TotalOrders = c.Orders.Count,
                    TotalSpent = c.Orders.Sum(o => (decimal?)o.TotalAmount) ?? 0
                })
                .ToList();
        }

    }
}
