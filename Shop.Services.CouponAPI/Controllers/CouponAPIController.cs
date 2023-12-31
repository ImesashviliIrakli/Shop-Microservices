﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Services.CouponAPI.Models;
using Shop.Services.CouponAPI.Models.Dto;
using Shop.Services.CouponAPI.Repositories;
using Shop.Services.CouponAPI.Utility;

namespace Shop.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CouponAPIController : ControllerBase
    {
        private readonly ICouponRepository _repo;
        private readonly IMapper _mapper;
        private ResponseDto _response;
        public CouponAPIController(ICouponRepository repo, IMapper mapper)
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
                IEnumerable<Coupon> couponList = _repo.GetCoupons();
                _response.Result = _mapper.Map<IEnumerable<CouponDto>>(couponList);
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
                Coupon coupon = _repo.GetCouponById(id);

                _response.Result = _mapper.Map<CouponDto>(coupon);

                if (_response.Result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Could not find Coupon";
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
        [Route("GetByCode/{code}")]
        public ResponseDto GetByCode(string code)
        {
            try
            {
                Coupon coupon = _repo.GetCouponByCode(code);

                _response.Result = _mapper.Map<CouponDto>(coupon);

                if (_response.Result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Could not find Coupon";
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
        public ResponseDto Post([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon coupon = _mapper.Map<Coupon>(couponDto);

                Coupon addedCoupon = _repo.Add(coupon);

                var options = new Stripe.CouponCreateOptions
                {
                    AmountOff = (long)(couponDto.DiscountAmount * 100),
                    Name = couponDto.CouponCode,
                    Currency = "usd",
                    Id = couponDto.CouponCode
                };

                var service = new Stripe.CouponService();
                service.Create(options);

                _response.Result = _mapper.Map<CouponDto>(addedCoupon);

                if (_response.Result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Could not add coupon";
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
        public ResponseDto Put([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon coupon = _mapper.Map<Coupon>(couponDto);

                Coupon updatedCoupon = _repo.Update(coupon);

                _response.Result = _mapper.Map<CouponDto>(updatedCoupon);

                if (_response.Result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Could not update coupon";
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
                Coupon deletedCoupon = _repo.Delete(id);

                var service = new Stripe.CouponService();

                service.Delete(deletedCoupon.CouponCode);

                _response.Result = _mapper.Map<CouponDto>(deletedCoupon);

                if (_response.Result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Could not remove coupon";
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
