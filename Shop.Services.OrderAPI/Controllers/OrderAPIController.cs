using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Services.OrderAPI.Data;
using Shop.Services.OrderAPI.Models;
using Shop.Services.OrderAPI.Models.Dto;
using Shop.Services.OrderAPI.Service.IService;
using Shop.Services.OrderAPI.Utility;
using System.Security.AccessControl;

namespace Shop.Services.OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderAPIController : ControllerBase
    {
        protected ResponseDto _response;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly IProductService _productService;

        public OrderAPIController(IMapper mapper, AppDbContext context, IProductService productService)
        {
            _mapper = mapper;
            _context = context;
            _productService = productService;
            _response = new ResponseDto();
        }

        [HttpPost("CreateOrder")]
        public async Task<ResponseDto> CreateOrder([FromBody] CartDto cartDto)
        {
            try
            {
                OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(cartDto.CartHeader);
                orderHeaderDto.OrderTime = DateTime.Now;
                orderHeaderDto.Status = SD.Status_Pending;
                orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDto>>(cartDto.CartDetails);

                OrderHeader orderCreated = _context.OrderHeaders.Add(_mapper.Map<OrderHeader>(orderHeaderDto)).Entity;
                await _context.SaveChangesAsync();

                orderHeaderDto.OrderHeaderId = orderCreated.OrderHeaderId;
                _response.Result = orderHeaderDto;
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
