using Shop.Services.ProductAPI.Models;

namespace Shop.Services.ProductAPI.Repositories
{
    public interface IProductRepository
    {
        public IEnumerable<Product> GetProducts();
        public Product GetProductById(int id);
        public IEnumerable<Product> GetProductByName(string productName);
        public Product Add(Product Product);
        public Product Update(Product Product);
        public Product Delete(int id);
    }
}
