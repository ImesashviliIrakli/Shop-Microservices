using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.MessageBus;
using Shop.Services.OrderAPI.Models;
using Shop.Services.OrderAPI.Models.Dto;
using Shop.Services.OrderAPI.Repositories;
using Shop.Services.OrderAPI.Service.IService;
using Shop.Services.OrderAPI.Utility;
using Stripe;
using Stripe.Checkout;
using System.Formats.Asn1;

namespace Shop.Services.OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderAPIController : ControllerBase
    {
        protected ResponseDto _response;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageBus _messageBus;
        private IConfiguration _configuration;

        public OrderAPIController(
            IMapper mapper,
            IProductService productService,
            IOrderRepository orderRepository, 
            IMessageBus messageBus,
            IConfiguration configuration
            )
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _messageBus = messageBus;
            _configuration = configuration;
            _response = new ResponseDto();
        }

        [HttpGet("GetOrders/{userId}")]
        public ResponseDto Get(string userId = "")
        {
            try
            {
                IEnumerable<OrderHeader> objList;
                if (User.IsInRole(SD.RoleAdmin))
                {
                    objList = _orderRepository.GetAllOrders();
                }
                else
                {
                    objList = _orderRepository.GetAllOrdersForUser(userId);
                }

                _response.Result = _mapper.Map<IEnumerable<OrderHeaderDto>>(objList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpGet("GetOrder/{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                OrderHeader orderHeader = _orderRepository.GetOrderHeaderWithDetails(id);

                _response.Result = _mapper.Map<OrderHeaderDto>(orderHeader);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
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
                orderHeaderDto.OrderTotal = Math.Round(orderHeaderDto.OrderTotal, 2);

                OrderHeader orderHeader = _mapper.Map<OrderHeader>(orderHeaderDto);

                OrderHeader orderCreated = await _orderRepository.AddOrderHeader(orderHeader);

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

        [HttpPost("CreateStripeSession")]
        public async Task<ResponseDto> CreateStripeSession([FromBody] StripeRequestDto stripeRequestDto)
        {
            try
            {
                #region Stripe Configuration

                var options = new SessionCreateOptions
                {
                    SuccessUrl = stripeRequestDto.ApprovedUrl,
                    CancelUrl = stripeRequestDto.CancelUrl,
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment"
                };

                var discountsObj = new List<SessionDiscountOptions>()
                {
                    new SessionDiscountOptions
                    {
                        Coupon = stripeRequestDto.OrderHeader.CouponCode
                    }
                };

                foreach (var item in stripeRequestDto.OrderHeader.OrderDetails)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.ProductName
                            }
                        },
                        Quantity = item.Count
                    };

                    options.LineItems.Add(sessionLineItem);
                }

                if (stripeRequestDto.OrderHeader.Discount > 0)
                {
                    options.Discounts = discountsObj;
                }

                var service = new SessionService();
                Session session = service.Create(options);

                stripeRequestDto.StripeSessionUrl = session.Url;

                #endregion

                OrderHeader orderHeader = await _orderRepository.GetOrderHeader(stripeRequestDto.OrderHeader.OrderHeaderId);

                orderHeader.StripeSessionsId = session.Id;

                await _orderRepository.UpdateOrderHeader(orderHeader);

                _response.Result = stripeRequestDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPost("ValidateStripeSession")]
        public async Task<ResponseDto> ValidateStripeSession([FromBody] int orderHeaderId)
        {
            try
            {
                OrderHeader orderHeader = await _orderRepository.GetOrderHeader(orderHeaderId);

                var service = new SessionService();
                Session session = service.Get(orderHeader.StripeSessionsId);

                var paymentIntentService = new PaymentIntentService();

                PaymentIntent paymentIntent = paymentIntentService.Get(session.PaymentIntentId);

                if (paymentIntent.Status == "succeeded")
                {
                    orderHeader.PaymentIntentId = paymentIntent.Id;
                    orderHeader.Status = SD.Status_Approved;

                    RewardsDto rewardsDto = new()
                    {
                        OrderId = orderHeader.OrderHeaderId,
                        RewardsActivity = Convert.ToInt32(orderHeader.OrderTotal),
                        UserId = orderHeader.UserId
                    };

                    string topicName = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");

                    await _messageBus.PublishMessage(rewardsDto, topicName);

                    await _orderRepository.UpdateOrderHeader(orderHeader);
                }

                _response.Result = orderHeader;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPost("UpdateOrderStatus/{orderId:int}")]
        public async Task<ResponseDto> UpdateOrderStatus(int orderId, [FromBody] string newStatus)
        {
            try
            {
                OrderHeader orderHeader = await _orderRepository.GetOrderHeader(orderId);

                if(orderHeader != null)
                {
                    if(newStatus == SD.Status_Cancelled)
                    {
                        var options = new RefundCreateOptions
                        {
                            Reason = RefundReasons.RequestedByCustomer,
                            PaymentIntent = orderHeader.PaymentIntentId
                        };

                        var service = new RefundService();

                        Refund refund = service.Create(options);
                    }

                    orderHeader.Status = newStatus;

                    await _orderRepository.UpdateOrderHeader(orderHeader);
                }

                _response.Result = true;
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
