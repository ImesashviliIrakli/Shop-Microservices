using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop.Web.Models;
using Shop.Web.Service.IService;
using Shop.Web.Utility;

namespace Shop.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text = SD.RoleAdmin, Value = SD.RoleAdmin },
                new SelectListItem{Text = SD.RoleCustomer, Value = SD.RoleCustomer },
            };

            ViewBag.RoleList = roleList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto registrationRequestDto)
        {
            ResponseDto register = await _authService.RegisterAsync(registrationRequestDto);

            if (register != null && register.IsSuccess)
            {
                if (string.IsNullOrEmpty(registrationRequestDto.RoleName))
                {
                    registrationRequestDto.RoleName = SD.RoleCustomer;
                }

                var assignRole = await _authService.AssignRoleAsync(registrationRequestDto);

                if(assignRole != null && assignRole.IsSuccess)
                {
                    TempData["success"] = "Registration Successful";

                    return RedirectToAction(nameof(Login));
                }
            }

            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text = SD.RoleAdmin, Value = SD.RoleAdmin },
                new SelectListItem{Text = SD.RoleCustomer, Value = SD.RoleCustomer },
            };

            ViewBag.RoleList = roleList;

            return View(registrationRequestDto);
        }

        public IActionResult Logout()
        {
            return View();
        }
    }
}
