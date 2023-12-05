using Shop.Web.Models;

namespace Shop.Web.Service.IService
{
    public interface ICartService
    {
        public Task<ResponseDto> GetCartByUserIdAsync(string userId);
        public Task<ResponseDto> UpsertCartAsync(CartDto cartDto);
        public Task<ResponseDto> RemoveFromCartAsync(int cartDetailsId);
        public Task<ResponseDto> RemoveEntireCartAsync(int cartHeaderId);
        public Task<ResponseDto> ApplyCouponAsync(CartDto cartDto);
        public Task<ResponseDto> EmailCart(CartDto cartDto);
    }
}
