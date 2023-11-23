using Shop.Web.Models;
using Shop.Web.Service.IService;
using Shop.Web.Utility;
using static Shop.Web.Utility.SD;

namespace Shop.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;
        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> GetAllCouponsAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = SD.CouponAPIBase + "/api/CouponAPI"
            });
        }

        public async Task<ResponseDto> GetCouponAsync(string couponCode)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = SD.CouponAPIBase + $"/api/CouponAPI/GetByCode/{couponCode}"
            });
        }

        public async Task<ResponseDto> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = SD.CouponAPIBase + $"/api/CouponAPI/{id}"
            });
        }

        public async Task<ResponseDto> CreateCouponAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Url = SD.CouponAPIBase + $"/api/CouponAPI",
                Data = couponDto
            });
        }

        public async Task<ResponseDto> UpdateCouponAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.PUT,
                Url = SD.CouponAPIBase + $"/api/CouponAPI",
                Data = couponDto
            });
        }

        public async Task<ResponseDto> DeleteCouponAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.DELETE,
                Url = SD.CouponAPIBase + $"/api/CouponAPI/{id}"
            });
        }
    }
}
