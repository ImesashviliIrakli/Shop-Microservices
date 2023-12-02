using Newtonsoft.Json;
using Shop.Services.ShoppingCartAPI.Models.Dto;
using Shop.Services.ShoppingCartAPI.Service.IService;

namespace Shop.Services.ShoppingCartAPI.Service
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CouponDto> GetCoupon(string couponCode)
        {
            var client = _httpClientFactory.CreateClient("Coupon");
            var response = await client.GetAsync($"/api/CouponAPI/GetByCode/{couponCode}");
            var apiContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if (result.Result != null && result.IsSuccess)
            {
                string resultString = Convert.ToString(result.Result);
                return JsonConvert.DeserializeObject<CouponDto>(resultString);
            }

            return new CouponDto();
        }
    }
}
