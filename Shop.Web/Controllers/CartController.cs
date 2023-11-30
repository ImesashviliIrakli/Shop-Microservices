using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.Web.Models;
using Shop.Web.Service.IService;
using System.IdentityModel.Tokens.Jwt;

namespace Shop.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            CartDto cartDto = await LoadCartDtoBasedOnLoggedInUser();
            return View(cartDto);
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
