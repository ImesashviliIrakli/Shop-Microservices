using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.Web.Models;
using Shop.Web.Service.IService;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace Shop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        public HomeController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto> products = new();

            ResponseDto response = await _productService.GetAllProductsAsync();

            if (response != null && response.IsSuccess)
            {
                string resultString = Convert.ToString(response.Result);

                products = JsonConvert.DeserializeObject<List<ProductDto>>(resultString);
            }
            else
            {
                TempData["error"] = response.Message;
            }

            return View(products);
        }

        [Authorize]
        public async Task<IActionResult> ProductDetails(int productId)
        {
            ProductDto products = new();

            ResponseDto response = await _productService.GetProductByIdAsync(productId);

            if (response != null && response.IsSuccess)
            {
                string resultString = Convert.ToString(response.Result);

                products = JsonConvert.DeserializeObject<ProductDto>(resultString);
            }
            else
            {
                TempData["error"] = response.Message;
            }

            return View(products);
        }

        [Authorize]
        [HttpPost]
        [ActionName("ProductDetails")]
        public async Task<IActionResult> ProductDetails(ProductDto productDto)
        {
            string userId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault().Value;

            CartDto cartDto = new();

            CartHeaderDto cartHeader = new() { UserId = userId };

            CartDetailsDto cartDetails = new()
            {
                Count = productDto.Count,
                ProductId = productDto.ProductId
            };
            List<CartDetailsDto> cartDetailsDtos = new() { cartDetails };

            cartDto.CartHeader = cartHeader;
            cartDto.CartDetails = cartDetailsDtos;

            ResponseDto response = await _cartService.UpsertCartAsync(cartDto);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Added to cart";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response.Message;
            }

            return View(productDto);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
