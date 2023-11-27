using Shop.Web.Models;
using Shop.Web.Models.Dto;
using Shop.Web.Service.IService;
using Shop.Web.Utility;
using static Shop.Web.Utility.SD;

namespace Shop.Web.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;
        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> GetAllProductsAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = SD.CouponAPIBase + "/api/ProductAPI"
            });
        }

        public async Task<ResponseDto> GetProductAsync(string productName)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = SD.CouponAPIBase + $"/api/ProductAPI/GetByName/{productName}"
            });
        }

        public async Task<ResponseDto> GetProductByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = SD.CouponAPIBase + $"/api/ProductAPI/{id}"
            });
        }

        public async Task<ResponseDto> CreateProductAsync(ProductDto ProductDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Url = SD.CouponAPIBase + $"/api/ProductAPI",
                Data = ProductDto
            });
        }

        public async Task<ResponseDto> UpdateProductAsync(ProductDto ProductDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.PUT,
                Url = SD.CouponAPIBase + $"/api/ProductAPI",
                Data = ProductDto
            });
        }

        public async Task<ResponseDto> DeleteProductAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.DELETE,
                Url = SD.CouponAPIBase + $"/api/ProductAPI/{id}"
            });
        }
    }
}
