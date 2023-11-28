using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.Web.Models;
using Shop.Web.Service.IService;
using System.Diagnostics;

namespace Shop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        public HomeController(IProductService productService)
        {
            _productService = productService;
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
