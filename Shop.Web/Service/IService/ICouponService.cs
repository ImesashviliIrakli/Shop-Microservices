using Shop.Web.Models;

namespace Shop.Web.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDto> GetAllCouponsAsync();
        Task<ResponseDto> GetCouponAsync(string couponCode);
        Task<ResponseDto> GetCouponByIdAsync(int id);
        Task<ResponseDto> CreateCouponAsync(CouponDto couponDto);
        Task<ResponseDto> UpdateCouponAsync(CouponDto couponDto);
        Task<ResponseDto> DeleteCouponAsync(int id);
    }
}
