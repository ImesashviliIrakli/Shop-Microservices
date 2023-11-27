using Microsoft.EntityFrameworkCore;
using Shop.Services.ProductAPI.Data;
using Shop.Services.ProductAPI.Models;
using System.Runtime.InteropServices.Marshalling;

namespace Shop.Services.ProductAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetProducts()
        {
            return _context.Products.ToList();
        }
        public Product GetProductById(int id)
        {
            return _context.Products.FirstOrDefault(x => x.ProductId == id);
        }

        public IEnumerable<Product> GetProductByName(string productName)
        {
            return _context.Products.Where(x => x.Name.Contains(productName)).ToList();
        }

        public Product Add(Product Product)
        {
            _context.Products.Add(Product);
            _context.SaveChanges();

            return Product;
        }

        public Product Update(Product Product)
        {
            _context.Products.Update(Product);
            _context.SaveChanges();

            return Product;
        }

        public Product Delete(int id)
        {
            Product product = _context.Products.FirstOrDefault(x => x.ProductId == id);

            if(product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }

            return product;
        }
    }
}
