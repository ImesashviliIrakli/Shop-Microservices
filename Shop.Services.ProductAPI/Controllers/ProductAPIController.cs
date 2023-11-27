using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Services.ProductAPI.Models;
using Shop.Services.ProductAPI.Models.Dto;
using Shop.Services.ProductAPI.Repositories;

namespace Shop.Services.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Post([FromBody] ProductDto ProductDto)
        {
            try
            {
                Product Product = _mapper.Map<Product>(ProductDto);

                Product addedProduct = _repo.Add(Product);

                _response.Result = _mapper.Map<ProductDto>(addedProduct);

                if (_response.Result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Could not add";
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
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Put([FromBody] ProductDto ProductDto)
        {
            try
            {
                Product Product = _mapper.Map<Product>(ProductDto);

                Product updatedProduct = _repo.Update(Product);

                _response.Result = _mapper.Map<ProductDto>(updatedProduct);

                if (_response.Result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Could not add";
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
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Product deletedProduct = _repo.Delete(id);

                _response.Result = _mapper.Map<ProductDto>(deletedProduct);

                if (_response.Result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Could not add";
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
