using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Shop.Services.EmailAPI.Data;
using Shop.Services.EmailAPI.Models;
using Shop.Services.EmailAPI.Models.Dto;
using Shop.Services.EmailAPI.Service.IService;
using System.Text;

namespace Shop.Services.EmailAPI.Service
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<AppDbContext> _dbOptions { get; }

        public EmailService(DbContextOptions<AppDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task NewUserLog(string email)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("New User Registered: " + email);

            await LogAndEmail(message.ToString(), email);
        }

        public async Task EmailCartAndLog(CartDto cartDto)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("<br/> Cart Email Requested");
            message.AppendLine("<br/>Total " + cartDto.CartHeader.CartTotal);
            message.Append("<br/>");
            message.Append("<ul>");

            foreach(var item in cartDto.CartDetails)
            {
                message.Append("<li>");
                message.Append(item.Product.Name + " x " + item.Count);
                message.Append("</li>");
            }

            message.Append("</ul>");

            await LogAndEmail(message.ToString(), cartDto.CartHeader.Email);
        }

        private async Task<bool> LogAndEmail(string message, string email)
        {
            try
            {
                EmailLogger emailLog = new()
                {
                    Email = email,
                    Message = message,
                    EmailSent = DateTime.Now
                };

                await using var _db = new AppDbContext(_dbOptions);

                await _db.EmailLoggers.AddAsync(emailLog);
                await _db.SaveChangesAsync();
                
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

       
    }
}
