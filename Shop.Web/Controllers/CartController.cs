using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.Web.Models;
using Shop.Web.Service.IService;
using System.IdentityModel.Tokens.Jwt;

namespace Shop.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> CartIndex()
        {
            CartDto cartDto = await LoadCartDtoBasedOnLoggedInUser();
            return View(cartDto);
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            ResponseDto response = await _cartService.RemoveFromCartAsync(cartDetailsId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Cart updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            ResponseDto response = await _cartService.ApplyCouponAsync(cartDto);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon applied successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            else
            {
                TempData["error"] = response.Message;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmailCart()
        {
            CartDto cart = await LoadCartDtoBasedOnLoggedInUser();
            cart.CartHeader.Email = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Email).FirstOrDefault().Value;

            ResponseDto response = await _cartService.EmailCart(cart);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Email will be processed and sent shortly";
                return RedirectToAction(nameof(CartIndex));
            }
            else
            {
                TempData["error"] = response.Message;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            cartDto.CartHeader.CouponCode = string.Empty;

            ResponseDto response = await _cartService.ApplyCouponAsync(cartDto);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon applied successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            else
            {
                TempData["error"] = response.Message;
            }

            return View();
        }

        private async Task<CartDto> LoadCartDtoBasedOnLoggedInUser() 
        {
            string userId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault().Value;

            ResponseDto response = await _cartService.GetCartByUserIdAsync(userId);

            if(response != null && response.IsSuccess)
            {
                string resultString = Convert.ToString(response.Result);
                CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(resultString);

                return cartDto;
            }

            return new CartDto();


        }
    }
}
