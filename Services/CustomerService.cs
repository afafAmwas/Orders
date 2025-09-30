using Microsoft.EntityFrameworkCore;
using Orders.DataAccess;
using Orders.Dtos;
using Orders.Models;

namespace Orders.Services
{
    public class CustomerService
    {
        private readonly Repository<Customer> _customerRepository;

        public CustomerService(Repository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public List<Customer> GetAllCustomers()
        {
            return _customerRepository.GetAll().ToList();
        }

        public Customer? GetCustomerById(int id)
        {
            return _customerRepository.GetById(id);
        }

        public Customer AddCustomer(Customer customer)
        {
            _customerRepository.Add(customer);
            _customerRepository.Save();
            return customer;
        }

        public bool UpdateCustomer(int id, Customer customer)
        {
            var existing = _customerRepository.GetById(id);
            if (existing == null)
                return false;

            existing.FullName = customer.FullName;
            _customerRepository.Save();
            return true;
        }

        public bool DeleteCustomer(int id)
        {
            var existing = _customerRepository.GetById(id);
            if (existing == null)
                return false;

            _customerRepository.Delete(existing);
            _customerRepository.Save();
            return true;
        }

        ///////////////////////////////////////////////////////////////////////- tasks start here

        public List<Customer> GetCustomersWithNoOrders()
        {
            return _customerRepository.QueryWithInclude(c => c.Orders)
                .Where(c => !c.Orders.Any())
                .ToList();
        }

        public List<CustomerAverageDto> GetCustomerAverageOrderValue()
        {
            return _customerRepository.QueryWithInclude(c => c.Orders)
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
            return _customerRepository.QueryWithInclude(c => c.Orders)
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
            return _customerRepository.QueryWithInclude(c => c.Orders)
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
