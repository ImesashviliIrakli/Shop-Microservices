using Shop.Service.EmailAPI.Message;
using Shop.Services.EmailAPI.Models.Dto;

namespace Shop.Services.EmailAPI.Service.IService
{
    public interface IEmailService
    {
        public Task NewUserLog(string email);
        public Task EmailCartAndLog(CartDto cartDto);
        public Task LogOrderPlaced(RewardsMessage rewardsMessage);
    }
}
