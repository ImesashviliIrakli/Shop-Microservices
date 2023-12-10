using Shop.Web.Models;

namespace Shop.Web.Service.IService
{
    public interface IOrderService
    {
        public Task<ResponseDto> CreateOrder(CartDto cartDto);
        public Task<ResponseDto> CreateStripeSession(StripeRequestDto stripeRequestDto);
        public Task<ResponseDto> ValidateStripeSession(int orderHeaderId);
        public Task<ResponseDto> GetAllOrderForUser(string userId);
        public Task<ResponseDto> GetOrder(int orderId);
        public Task<ResponseDto> UpdateOrderStatus(int orderId, string newStatus);

    }
}
