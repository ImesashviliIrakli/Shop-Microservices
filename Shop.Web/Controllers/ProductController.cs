using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.Web.Models;
using Shop.Web.Service.IService;

namespace Shop.Web.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductService _productService;
		public ProductController(IProductService productService)
		{
			_productService = productService;
		}

		public async Task<IActionResult> ProductIndex()
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

			return View(products); ;
		}

		public async Task<IActionResult> ProductCreate(ProductDto productDto)
		{
			if (ModelState.IsValid)
			{
				ResponseDto response = await _productService.CreateProductAsync(productDto);

				if (response != null && response.IsSuccess)
				{
					TempData["success"] = "Product Created Successfully";

					return RedirectToAction(nameof(ProductIndex));
				}
				else
				{
					TempData["error"] = response.Message;
				}
			}

			return View(productDto);
		}

		public async Task<IActionResult> ProductUpdate(int id)
		{
			ProductDto productDto = new();

			ResponseDto response = await _productService.GetProductByIdAsync(id);

			if (response != null && response.IsSuccess)
			{
				string result = Convert.ToString(response.Result);
				productDto = JsonConvert.DeserializeObject<ProductDto>(result);
			}
			else
			{
				TempData["error"] = response.Message;
			}

			return View(productDto);
		}

		[HttpPost]
		public async Task<IActionResult> ProductUpdate(ProductDto productDto)
		{
			if (ModelState.IsValid)
			{
				ResponseDto response = await _productService.UpdateProductAsync(productDto);

				if (response != null && response.IsSuccess)
				{
					TempData["success"] = "Product Updated Successfully";

					return RedirectToAction(nameof(ProductIndex));
				}
				else
				{
					TempData["error"] = response.Message;
				}
			}

			return View(productDto);
		}

		public async Task<IActionResult> ProductDelete(int id)
		{
			ProductDto productDto = new();

			ResponseDto response = await _productService.GetProductByIdAsync(id);

			if (response != null && response.IsSuccess)
			{
				string result = Convert.ToString(response.Result);
				productDto = JsonConvert.DeserializeObject<ProductDto>(result);
			}
			else
			{
				TempData["error"] = response.Message;
			}

			return View(productDto);
		}

		[HttpPost]

		public async Task<IActionResult> ProductDelete(ProductDto productDto)
		{
			if (ModelState.IsValid)
			{
				ResponseDto response = await _productService.DeleteProductAsync(productDto.ProductId);

				if (response != null && response.IsSuccess)
				{
					TempData["success"] = "Product Deleted Successfully";

					return RedirectToAction(nameof(ProductIndex));
				}
				else
				{
					TempData["error"] = response.Message;
				}
			}

			return View(productDto);
		}
	}
}
