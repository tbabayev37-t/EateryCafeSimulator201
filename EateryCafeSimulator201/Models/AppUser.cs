using Microsoft.AspNetCore.Identity;

namespace EateryCafeSimulator201.Models
{
    public class AppUser:IdentityUser
    {
        public string Fullname { get; set; } = null!;
    }
}
