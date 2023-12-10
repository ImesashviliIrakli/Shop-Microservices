﻿using Shop.Web.Models;
using Shop.Web.Service.IService;
using Shop.Web.Utility;
using static Shop.Web.Utility.SD;

namespace Shop.Web.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBaseService _baseService;
        public OrderService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> CreateOrder(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Url = SD.OrderAPIBase + $"/api/OrderAPI/CreateOrder",
                Data = cartDto
            });
        }

        public async Task<ResponseDto> CreateStripeSession(StripeRequestDto stripeRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Url = SD.OrderAPIBase + $"/api/OrderAPI/CreateStripeSession",
                Data = stripeRequestDto
            });
        }

        public async Task<ResponseDto> GetAllOrderForUser(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = SD.OrderAPIBase + $"/api/OrderAPI/GetOrders/{userId}",
            });
        }

        public async Task<ResponseDto> GetOrder(int orderId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = SD.OrderAPIBase + $"/api/OrderAPI/GetOrder/{orderId}",
            });
        }

        public async Task<ResponseDto> UpdateOrderStatus(int orderId, string newStatus)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Url = SD.OrderAPIBase + $"/api/OrderAPI/UpdateOrderStatus/{orderId}",
                Data = newStatus
            });
        }

        public async Task<ResponseDto> ValidateStripeSession(int orderHeaderId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Url = SD.OrderAPIBase + $"/api/OrderAPI/ValidateStripeSession",
                Data = orderHeaderId
            });
        }
    }
}
