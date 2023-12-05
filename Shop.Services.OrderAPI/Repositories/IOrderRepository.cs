using Shop.Services.OrderAPI.Models;
using Shop.Services.OrderAPI.Models.Dto;

namespace Shop.Services.OrderAPI.Repositories
{
    public interface IOrderRepository
    {
        public Task<OrderHeader> GetOrderHeader(int orderHeaderId);
        public Task<OrderHeader> AddOrderHeader(OrderHeader orderHeader);
        public Task<OrderDetails> AddOrderDetails(OrderDetails orderDetails);
        public Task<OrderHeader> UpdateOrderHeader(OrderHeader orderHeader);
    }
}
