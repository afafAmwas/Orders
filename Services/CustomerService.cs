using Orders.DataAccess;
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
    }
}
