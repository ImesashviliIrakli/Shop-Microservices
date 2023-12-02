using Shop.Services.EmailAPI.Models.Dto;

namespace Shop.Services.EmailAPI.Service.IService
{
    public interface IEmailService
    {
        public Task NewUserLog(string email);
        public Task EmailCartAndLog(CartDto cartDto);
    }
}
