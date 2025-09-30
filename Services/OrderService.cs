using Microsoft.EntityFrameworkCore;
using Orders.DataAccess;
using Orders.Dtos;
using Orders.Models;

namespace Orders.Services
{
    public class OrderService
    {
        private readonly Repository<Order> _orderRepository;

        public OrderService(Repository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public List<OrderReadDto> GetAllOrders()
        {
            return _orderRepository.GetAll()
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
            var order = _orderRepository.GetById(id);
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

            _orderRepository.Add(order);
            _orderRepository.Save();

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
            var existing = _orderRepository.GetById(id);
            if (existing == null)
                return false;

            existing.CustomerId = dto.CustomerId;
            existing.TotalAmount = dto.TotalAmount;
            existing.OrderDate = dto.OrderDate;

            _orderRepository.Save();
            return true;
        }

        public bool DeleteOrder(int id)
        {
            var existing = _orderRepository.GetById(id);
            if (existing == null)
                return false;

            _orderRepository.Delete(existing);
            _orderRepository.Save();
            return true;
        }
        /////////////////////////////////////////////////////////////////////////- Tasks start here
        
        public List<MonthlyRevenueDto> GetMonthlyRevenue()
        {
            var currentYear = DateTime.Now.Year;

            var monthlyRevenue = _orderRepository.GetAll()
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
            return _orderRepository.QueryWithInclude(o => o.Customer)
            .GroupBy(o => o.Customer.FullName)
            .Select(g => new TopCustomerDto
            {
                FullName = g.Key,
                TotalSpent = g.Sum(o => o.TotalAmount)
            })
            .OrderByDescending(x => x.TotalSpent)
            .Take(top)
            .ToList();

        }

        public List<MonthlyProfitDto> GetMonthlyProfit(int year)
        {
            return _orderRepository.Query()
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
            var allOrders = _orderRepository.QueryWithInclude(o => o.Customer).ToList();

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
            var allOrders = _orderRepository.QueryWithInclude(o => o.Customer).ToList();

            return allOrders
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
        }
        public List<DailySummaryDto> GetDailyOrderSummary()
        {
            var allOrders = _orderRepository.Query().ToList();

            return allOrders
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
        }
    }
}
