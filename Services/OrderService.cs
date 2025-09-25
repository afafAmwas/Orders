using Orders.DataAccess;
using Orders.Models;
using Orders.Dtos;

namespace Orders.Services
{
    public class OrderService
    {
        private readonly Repository<Order> _repo;

        public OrderService(Repository<Order> repo)
        {
            _repo = repo;
        }

        public List<OrderReadDto> GetAllOrders()
        {
            return _repo.GetAll()
                        .Select(o => new OrderReadDto
                        {
                            Id = o.Id,
                            CustomerId = o.CustomerId,
                            TotalAmount = o.TotalAmount,
                            OrderDate = o.OrderDate
                        })
                        .ToList();
        }

        public OrderReadDto? GetOrderById(int id)
        {
            var order = _repo.GetById(id);
            if (order == null)
                return null;

            return new OrderReadDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                TotalAmount = order.TotalAmount,
                OrderDate = order.OrderDate
            };
        }

        public OrderReadDto AddOrder(OrderCreateUpdateDto dto)
        {
            var order = new Order
            {
                CustomerId = dto.CustomerId,
                TotalAmount = dto.TotalAmount,
                OrderDate = dto.OrderDate
            };

            _repo.Add(order);
            _repo.Save();

            return new OrderReadDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                TotalAmount = order.TotalAmount,
                OrderDate = order.OrderDate
            };
        }

        public bool UpdateOrder(int id, OrderCreateUpdateDto dto)
        {
            var existing = _repo.GetById(id);
            if (existing == null)
                return false;

            existing.CustomerId = dto.CustomerId;
            existing.TotalAmount = dto.TotalAmount;
            existing.OrderDate = dto.OrderDate;

            _repo.Save();
            return true;
        }

        public bool DeleteOrder(int id)
        {
            var existing = _repo.GetById(id);
            if (existing == null)
                return false;

            _repo.Delete(existing);
            _repo.Save();
            return true;
        }
        public List<MonthlyRevenueDto> GetMonthlyRevenue()
        {
            var currentYear = DateTime.Now.Year;

            var monthlyRevenue = _repo.GetAll()
                .Where(o => o.OrderDate.Year == currentYear)
                .GroupBy(o => o.OrderDate.Month)
                .Select(g => new MonthlyRevenueDto
                {
                    Month = g.Key,
                    TotalRevenue = g.Sum(o => o.TotalAmount)
                })
                .OrderBy(r => r.Month)
                .ToList();

            return monthlyRevenue;
        }
    }
}
