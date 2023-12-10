using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.Web.Models;
using Shop.Web.Service.IService;
using Shop.Web.Utility;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace Shop.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public IActionResult OrderIndex()
        {
            return View();
        }

        public async Task<IActionResult> OrderDetail(int orderId)
        {
            OrderHeaderDto orderHeaderDto = new OrderHeaderDto();

            string userId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault().Value;

            ResponseDto response = await _orderService.GetOrder(orderId);

            if (response != null && response.IsSuccess)
            {
                string resultString = Convert.ToString(response.Result);
                orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(resultString);
            }

            if (!User.IsInRole(SD.RoleAdmin) && userId != orderHeaderDto.UserId)
            {
                return NotFound();
            }

            return View(orderHeaderDto);
        }

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeaderDto> list;
            string userId = string.Empty;

            if (User.IsInRole(SD.RoleAdmin))
            {
                userId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault().Value;
            }

            ResponseDto response = _orderService.GetAllOrderForUser(userId).GetAwaiter().GetResult();

            if (response != null && response.IsSuccess)
            {
                string resultString = Convert.ToString(response.Result);
                list = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(resultString);

                switch(status)
                {
                    case "approved":
                        list = list.Where(x => x.Status == SD.Status_Approved).ToList();
                        break;
                    case "readyforpickup":
                        list = list.Where(x => x.Status == SD.Status_ReadyForPickup).ToList();
                        break;
                    case "cancelled":
                        list = list.Where(x => x.Status == SD.Status_Cancelled).ToList();
                        break;
                    default:
                        break;
                }

            }
            else
            {
                list = new List<OrderHeaderDto>();
            }

            return Json(new { data = list });
        }

        [HttpPost("OrderReadyForPickup")]
        public async Task<IActionResult> OrderReadyForPickup(int orderId)
        {
            ResponseDto response = await _orderService.UpdateOrderStatus(orderId, SD.Status_ReadyForPickup);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status updated successfully";

                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }

            return View();
        }

        [HttpPost("CompleteOrder")]
        public async Task<IActionResult> CompleteOrder(int orderId)
        {
            ResponseDto response = await _orderService.UpdateOrderStatus(orderId, SD.Status_Completed);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status updated successfully";

                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }

            return View();
        }

        [HttpPost("CancelOrder")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            ResponseDto response = await _orderService.UpdateOrderStatus(orderId, SD.Status_Cancelled);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status updated successfully";

                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }

            return View();
        }

    }
}
