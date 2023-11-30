using Shop.Services.ShoppingCartAPI.Models.Dto;

namespace Shop.Services.ShoppingCartAPI.Service.IService
{
    public interface ICouponService
    {
        public Task<CouponDto> GetCoupon(string couponCode);
    }
}
