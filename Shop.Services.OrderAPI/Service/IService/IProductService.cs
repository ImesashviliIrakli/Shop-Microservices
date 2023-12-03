using Shop.Services.OrderAPI.Models.Dto;

namespace Shop.Services.OrderAPI.Service.IService
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDto>> GetProducts();
    }
}
