using Shop.Web.Models;

namespace Shop.Web.Service.IService
{
	public interface IProductService
	{
		Task<ResponseDto> GetAllProductsAsync();
		Task<ResponseDto> GetProductAsync(string productName);
		Task<ResponseDto> GetProductByIdAsync(int id);
		Task<ResponseDto> CreateProductAsync(ProductDto ProductDto);
		Task<ResponseDto> UpdateProductAsync(ProductDto ProductDto);
		Task<ResponseDto> DeleteProductAsync(int id);
	}
}
