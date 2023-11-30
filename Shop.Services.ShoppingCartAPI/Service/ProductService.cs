using Newtonsoft.Json;
using Shop.Services.ShoppingCartAPI.Models.Dto;
using Shop.Services.ShoppingCartAPI.Service.IService;

namespace Shop.Services.ShoppingCartAPI.Service
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"/api/ProductAPI");
            var apiContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if (result.IsSuccess)
            {
                string resultString = Convert.ToString(result.Result);
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(resultString);
            }

            return new List<ProductDto>();
        }
    }
}
