using Microsoft.AspNetCore.Identity;

namespace JobHunt.Services.AuthAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
