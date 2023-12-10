using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Services.ProductAPI.Models;
using Shop.Services.ProductAPI.Models.Dto;
using Shop.Services.ProductAPI.Repositories;
using Shop.Services.ProductAPI.Utility;

namespace Shop.Services.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;
        private ResponseDto _response;
        public ProductAPIController(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Product> ProductList = _repo.GetProducts();
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(ProductList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Product Product = _repo.GetProductById(id);

                _response.Result = _mapper.Map<ProductDto>(Product);

                if (_response.Result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Could not find Product";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpGet]
        [Route("GetByName/{name}")]
        public ResponseDto GetByName(string name)
        {
            try
            {
                IEnumerable<Product> Product = _repo.GetProductByName(name);

                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(Product);

                if (_response.Result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Could not find Product";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPost]
        [Authorize(Roles = SD.RoleAdmin)]
        public ResponseDto Post(ProductDto ProductDto)
        {
            try
            {
                Product product = _mapper.Map<Product>(ProductDto);

                _repo.Add(product);

                if (ProductDto.Image != null)
                {

                    string fileName = product.ProductId + Path.GetExtension(ProductDto.Image.FileName);
                    string filePath = @"wwwroot\ProductImages\" + fileName;

                    //I have added the if condition to remove the any image with same name if that exist in the folder by any change
                    var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    FileInfo file = new FileInfo(directoryLocation);
                    if (file.Exists)
                    {
                        file.Delete();
                    }

                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        ProductDto.Image.CopyTo(fileStream);
                    }
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    product.ImageUrl = baseUrl + "/ProductImages/" + fileName;
                    product.ImageLocalPath = filePath;
                }
                else
                {
                    product.ImageUrl = "https://placehold.co/600x400";
                }

                _repo.Update(product);

                _response.Result = _mapper.Map<ProductDto>(product);

                if (_response.Result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Could not add product";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPut]
        [Authorize(Roles = SD.RoleAdmin)]
        public ResponseDto Put([FromBody] ProductDto productDto)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDto);

                if (productDto.Image != null)
                {
                    if (!string.IsNullOrEmpty(product.ImageLocalPath))
                    {
                        var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
                        FileInfo file = new FileInfo(oldFilePathDirectory);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }

                    string fileName = product.ProductId + Path.GetExtension(productDto.Image.FileName);
                    string filePath = @"wwwroot\ProductImages\" + fileName;
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        productDto.Image.CopyTo(fileStream);
                    }
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    product.ImageUrl = baseUrl + "/ProductImages/" + fileName;
                    product.ImageLocalPath = filePath;
                }

                _repo.Update(product);

                _response.Result = _mapper.Map<ProductDto>(product);

                if (_response.Result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Could not update product";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = SD.RoleAdmin)]
        public ResponseDto Delete(int id)
        {
            try
            {
                Product deletedProduct = _repo.Delete(id);

                if (!string.IsNullOrEmpty(deletedProduct.ImageLocalPath))
                {
                    var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), deletedProduct.ImageLocalPath);
                    FileInfo file = new FileInfo(oldFilePathDirectory);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }

                _response.Result = _mapper.Map<ProductDto>(deletedProduct);

                if (_response.Result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Could not delete product";
                }
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
