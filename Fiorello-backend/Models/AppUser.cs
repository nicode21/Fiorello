using Microsoft.AspNetCore.Identity;

namespace Fiorello_backend.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
