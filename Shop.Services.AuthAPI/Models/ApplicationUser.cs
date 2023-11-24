using Microsoft.AspNetCore.Identity;

namespace Shop.Services.AuthAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
