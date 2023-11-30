using Shop.Services.ShoppingCartAPI.Models.Dto;

namespace Shop.Services.ShoppingCartAPI.Service.IService
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDto>> GetProducts();
    }
}
