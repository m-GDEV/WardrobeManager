using Microsoft.AspNetCore.Identity;

namespace WardrobeManager.Api.Database.Models;

public class AppUser : IdentityUser
{
   public IEnumerable<IdentityRole>? Roles { get; set; }
}