using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Services.ShoppingCartAPI.Data;
using Shop.Services.ShoppingCartAPI.Models;
using Shop.Services.ShoppingCartAPI.Models.Dto;
using System.Collections.Frozen;

namespace Shop.Services.ShoppingCartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private ResponseDto _response;
        public CartAPIController(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
            _response = new();
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDto cartDto)
        {
            try
            {
                var cartHeaderFromDb = await _context.CartHeaders.FirstOrDefaultAsync(x => x.UserId == cartDto.CartHeader.UserId);
                
                if(cartHeaderFromDb == null)
                {
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                    _context.CartHeaders.Add(cartHeader);
                    _context.SaveChanges();
                    cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;

                    CartDetails cartDetails = _mapper.Map<CartDetails>(cartDto.CartDetails.First());
                    _context.CartDetails.Add(cartDetails);
                    _context.SaveChanges();
                }
                else
                {
                    var cartDetailsFrombDb = await _context.CartDetails
                        .FirstOrDefaultAsync(
                            x => x.ProductId == cartDto.CartDetails.First().ProductId && 
                            x.CartHeaderId == cartHeaderFromDb.CartHeaderId
                        );

                    if(cartDetailsFrombDb == null)
                    {

                    }
                    else
                    {

                    }

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
