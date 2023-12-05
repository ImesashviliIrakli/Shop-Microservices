using Microsoft.EntityFrameworkCore;
using Shop.Services.OrderAPI.Data;
using Shop.Services.OrderAPI.Models;

namespace Shop.Services.OrderAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OrderHeader> GetOrderHeader(int orderHeaderId)
        {
            OrderHeader orderHeader = await _context.OrderHeaders.FirstOrDefaultAsync(x => x.OrderHeaderId == orderHeaderId);
            return orderHeader;
        }

        public async Task<OrderHeader> AddOrderHeader(OrderHeader orderHeader)
        {
            _context.OrderHeaders.Add(orderHeader);
            await _context.SaveChangesAsync();

            return orderHeader;
        }

        public async Task<OrderHeader> UpdateOrderHeader(OrderHeader orderHeader)
        {
            _context.OrderHeaders.Update(orderHeader);
            await _context.SaveChangesAsync();

            return orderHeader;
        }

        public async Task<OrderDetails> AddOrderDetails(OrderDetails orderDetails)
        {
            _context.OrderDetails.Add(orderDetails);
            await _context.SaveChangesAsync();

            return orderDetails;
        }
    }
}
