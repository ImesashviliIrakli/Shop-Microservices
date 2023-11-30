using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shop.Services.ShoppingCartAPI.Data;
using Shop.Services.ShoppingCartAPI.Models;
using Shop.Services.ShoppingCartAPI.Models.Dto;
using Shop.Services.ShoppingCartAPI.Repositories;
using Shop.Services.ShoppingCartAPI.Service.IService;

namespace Shop.Services.ShoppingCartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly ICartRepository _cartRepository;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;
        private ResponseDto _response;
        public CartAPIController(
            IMapper mapper,
            AppDbContext context,
            ICartRepository cartRepository,
            IProductService productService,
            ICouponService couponService)
        {
            _mapper = mapper;
            _context = context;
            _cartRepository = cartRepository;
            _productService = productService;
            _response = new();
            _couponService = couponService;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                CartHeader cartHeader = await _cartRepository.GetCartHeaderByUserId(userId);
                CartHeaderDto cartHeaderDto = _mapper.Map<CartHeaderDto>(cartHeader);

                IEnumerable<CartDetails> cartDetails = await _cartRepository.GetCartDetailsByHeader(cartHeader.CartHeaderId);
                IEnumerable<CartDetailsDto> cartDetailsDto = _mapper.Map<IEnumerable<CartDetailsDto>>(cartDetails);

                IEnumerable<ProductDto> products = await _productService.GetProducts();

                CartDto cart = new()
                {
                    CartHeader = cartHeaderDto,
                    CartDetails = cartDetailsDto
                };

                foreach (var item in cart.CartDetails)
                {
                    item.Product = products.FirstOrDefault(x => x.ProductId == item.ProductId);
                    cart.CartHeader.CartTotal += (item.Count * item.Product.Price);
                }

                // Apply coupon logic
                if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                {
                    CouponDto coupon = await _couponService.GetCoupon(cart.CartHeader.CouponCode);

                    if (coupon != null && cart.CartHeader.CartTotal > coupon.MinAmount)
                    {
                        cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                        cart.CartHeader.Discount = coupon.DiscountAmount;
                    }
                }

                _response.Result = cart;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<ResponseDto> ApplyCoupon(CartDto cartDto)
        {
            try
            {
                var cartFromDb = await _cartRepository.GetCartHeaderByUserId(cartDto.CartHeader.UserId);

                cartFromDb.CouponCode = cartDto.CartHeader.CouponCode;

                await _cartRepository.UpdateCartHeader(cartFromDb);

                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPost("RemoveCoupon")]
        public async Task<ResponseDto> RemoveCoupon(CartDto cartDto)
        {
            try
            {
                var cartFromDb = await _cartRepository.GetCartHeaderByUserId(cartDto.CartHeader.UserId);

                cartFromDb.CouponCode = string.Empty;

                await _cartRepository.UpdateCartHeader(cartFromDb);

                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDto cartDto)
        {
            try
            {
                var cartHeaderFromDb = await _cartRepository.GetCartHeaderByUserId(cartDto.CartHeader.UserId);

                if (cartHeaderFromDb == null)
                {
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                    _cartRepository.AddCartHeader(cartHeader);

                    cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;

                    CartDetails cartDetails = _mapper.Map<CartDetails>(cartDto.CartDetails.First());

                    await _cartRepository.AddCartDetails(cartDetails);
                }
                else
                {
                    int productId = cartDto.CartDetails.First().ProductId;
                    int cartHeaderId = cartHeaderFromDb.CartHeaderId;

                    var cartDetailsFromDb = await _cartRepository.GetCartDetails(productId, cartHeaderId);

                    if (cartDetailsFromDb == null)
                    {
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderId;
                        CartDetails cartDetails = _mapper.Map<CartDetails>(cartDto.CartDetails.First());

                        await _cartRepository.AddCartDetails(cartDetails);
                    }
                    else
                    {
                        cartDto.CartDetails.First().Count += cartDetailsFromDb.Count;
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderId;
                        cartDto.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;

                        await _cartRepository.UpdateCartDetails(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                    }
                }

                _response.Result = cartDto;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody] int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = _cartRepository.GetCartDetailsById(cartDetailsId);

                int totalCartItemCount = _cartRepository.GetCartItemCountByHeader(cartDetails.CartHeaderId);

                await _cartRepository.RemoveCartDetails(cartDetails);

                if (totalCartItemCount == 1)
                {
                    await _cartRepository.RemoveCartHeader(cartDetails.CartHeaderId);
                }

                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

    }
}
