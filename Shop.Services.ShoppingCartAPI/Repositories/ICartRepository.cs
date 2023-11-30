using Shop.Services.ShoppingCartAPI.Models;

namespace Shop.Services.ShoppingCartAPI.Repositories
{
    public interface ICartRepository
    {
        public Task<CartHeader> GetCartHeaderByUserId(string userId);
        public Task<CartHeader> GetCartHeaderById(int cartHeaderId);
        public Task<CartDetails> GetCartDetails(int productId, int cartHeaderId);
        public Task<IEnumerable<CartDetails>> GetCartDetailsByHeader(int cartHeaderId);
        public CartDetails GetCartDetailsById(int cartDetailsId);
        public int GetCartItemCountByHeader(int cartHeaderId);
        public CartHeader AddCartHeader(CartHeader cartHeader);
        public Task<CartDetails> AddCartDetails(CartDetails cartDetails);
        public Task<CartDetails> UpdateCartDetails(CartDetails cartDetails);
        public Task<CartHeader> RemoveCartHeader(int cartHeaderId);
        public Task<CartDetails> RemoveCartDetails(CartDetails cartDetails);
    }
}
