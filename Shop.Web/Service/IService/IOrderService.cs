using Shop.Web.Models;

namespace Shop.Web.Service.IService
{
    public interface IOrderService
    {
        public Task<ResponseDto> CreateOrder(CartDto cartDto);
    }
}
