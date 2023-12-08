using Shop.Services.OrderAPI.Models;

namespace Shop.Services.OrderAPI.Repositories
{
    public interface IOrderRepository
    {
        public List<OrderHeader> GetAllOrders();
        public List<OrderHeader> GetAllOrdersForUser(string userId);
        public Task<OrderHeader> GetOrderHeader(int orderHeaderId);
        public OrderHeader GetOrderHeaderWithDetails(int orderHeaderId);
        public Task<OrderHeader> AddOrderHeader(OrderHeader orderHeader);
        public Task<OrderDetails> AddOrderDetails(OrderDetails orderDetails);
        public Task<OrderHeader> UpdateOrderHeader(OrderHeader orderHeader);
    }
}
