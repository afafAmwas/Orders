using Microsoft.EntityFrameworkCore;
using Orders.DataAccess;
using Orders.Dtos;
using Orders.Models;

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
                            OrderDate = o.OrderDate,
                            CostAmount = o.CostAmount
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
                OrderDate = order.OrderDate,
                CostAmount = order.CostAmount
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

        public List<TopCustomerDto> GetTopCustomersBySpending(int top = 5)
        {
            var query = _repo.Query()
                .Cast<Order>()                   
                .Include(o => o.Customer)         
                .GroupBy(o => o.Customer.FullName)
                .Select(g => new TopCustomerDto
                {
                    FullName = g.Key,
                    TotalSpent = g.Sum(o => o.TotalAmount)
                })
                .OrderByDescending(x => x.TotalSpent)
                .Take(top)
                .ToList();

            return query;
        }

        public List<MonthlyProfitDto> GetMonthlyProfit(int year)
        {
            return _repo.Query()
                .Cast<Order>()
                .Where(o => o.OrderDate.Year == year)
                .GroupBy(o => o.OrderDate.Month)
                .Select(g => new MonthlyProfitDto
                {
                    Month = g.Key,
                    Revenue = g.Sum(o => o.TotalAmount),
                    Cost = g.Sum(o => o.CostAmount),
                    Profit = g.Sum(o => o.TotalAmount) - g.Sum(o => o.CostAmount)
                })
                .OrderBy(r => r.Month)
                .ToList();
        }
        public List<OrderReadDto> GetOrdersAboveCustomerAverage()
        {
            var allOrders = _repo.GetAll().ToList(); // bring all orders into memory

            var result = allOrders
                .GroupBy(o => o.CustomerId)
                .SelectMany(g =>
                {
                    var avg = g.Average(o => o.TotalAmount);
                    return g.Where(o => o.TotalAmount > avg);
                })
                .Select(o => new OrderReadDto
                {
                    Id = o.Id,
                    CustomerId = o.CustomerId,
                    TotalAmount = o.TotalAmount,
                    CostAmount = o.CostAmount,
                    OrderDate = o.OrderDate
                })
                .ToList();

            return result;
        }

        public List<CustomerRecentOrderDto> GetMostRecentOrderPerCustomer()
        {
            var allOrders = _repo.Query()
                .Cast<Order>()
                .Include(o => o.Customer)
                .ToList(); 

            var result = allOrders
                .GroupBy(o => o.Customer.FullName) 
                .Select(g =>
                {
                    var recentOrder = g.OrderByDescending(o => o.OrderDate).First();
                    return new CustomerRecentOrderDto
                    {
                        CustomerName = g.Key,
                        OrderId = recentOrder.Id,
                        OrderDate = recentOrder.OrderDate,
                        TotalAmount = recentOrder.TotalAmount
                    };
                })
                .ToList();

            return result;
        }
        public List<DailySummaryDto> GetDailyOrderSummary()
        {
            // bring orders into memory first
            var allOrders = _repo.GetAll().ToList();

            var summary = allOrders
                .GroupBy(o => o.OrderDate.Date)       
                .Where(g => g.Count() >= 2)         
                .Select(g => new DailySummaryDto
                {
                    OrderDate = g.Key,
                    OrderCount = g.Count(),
                    DailyRevenue = g.Sum(o => o.TotalAmount)
                })
                .OrderBy(s => s.OrderDate)
                .ToList();

            return summary;
        }


    }
}
