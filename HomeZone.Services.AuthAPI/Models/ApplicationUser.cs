using Microsoft.AspNetCore.Identity;

namespace HomeZone.Services.AuthAPI.Models
{
    public class ApplicationUser: IdentityUser

    {
        public string Name {  get; set; }
    }
}
