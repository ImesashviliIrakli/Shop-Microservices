using Microsoft.EntityFrameworkCore;
using Shop.Services.ShoppingCartAPI.Data;
using Shop.Services.ShoppingCartAPI.Models;
using Shop.Services.ShoppingCartAPI.Models.Dto;

namespace Shop.Services.ShoppingCartAPI.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;
        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CartHeader> GetCartHeaderByUserId(string userId)
        {
            return await _context.CartHeaders.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<CartHeader> GetCartHeaderById(int cartHeaderId)
        {
            return await _context.CartHeaders.FirstOrDefaultAsync(x => x.CartHeaderId == cartHeaderId);
        }

        public async Task<CartDetails> GetCartDetails(int productId, int cartHeaderId)
        {
            return await _context.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                            x => x.ProductId == productId &&
                            x.CartHeaderId == cartHeaderId
                        );
        }

        public CartDetails GetCartDetailsById(int cartDetailsId)
        {
            return _context.CartDetails.First(x => x.CartDetailsId == cartDetailsId);
        }

        public async Task<IEnumerable<CartDetails>> GetCartDetailsByHeader(int cartHeaderId)
        {
            return _context.CartDetails.Where(x => x.CartHeaderId == cartHeaderId).ToList();
        }

        public int GetCartItemCountByHeader(int cartHeaderId)
        {
            return _context.CartDetails.Where(x => x.CartHeaderId == cartHeaderId).Count();
        }

        public CartHeader AddCartHeader(CartHeader cartHeader)
        {
            _context.CartHeaders.Add(cartHeader);
            _context.SaveChanges();

            return cartHeader;
        }

        public async Task<CartDetails> AddCartDetails(CartDetails cartDetails)
        {
            _context.CartDetails.Add(cartDetails);
            await _context.SaveChangesAsync();

            return cartDetails;
        }

        public async Task<CartDetails> UpdateCartDetails(CartDetails cartDetails)
        {
            _context.CartDetails.Update(cartDetails);
            await _context.SaveChangesAsync();

            return cartDetails;
        }

        public async Task<CartHeader> RemoveCartHeader(int cartHeaderId)
        {
            CartHeader cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(x => x.CartHeaderId == cartHeaderId);

            _context.CartHeaders.Remove(cartHeader);

            await _context.SaveChangesAsync();

            return cartHeader;
        }

        public async Task<CartDetails> RemoveCartDetails(CartDetails cartDetails)
        {
            _context.CartDetails.Remove(cartDetails);

            await _context.SaveChangesAsync();

            return cartDetails;
        }

        public async Task<CartDetails> UpdateCartHeader(CartHeader cartHeader)
        {
            _context.CartHeaders.Update(cartHeader);
            await _context.SaveChangesAsync();

            return cartHeader;
        }
    }
}
