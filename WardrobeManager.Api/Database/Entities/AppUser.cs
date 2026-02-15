#region

using Microsoft.AspNetCore.Identity;

#endregion

namespace WardrobeManager.Api.Database.Entities;

// As of right now, the AppUser class is only used for authentication. 
// The User class is used to connect an AppUser with User and give them a profile in the app
// In the future it might be a good idea to merge these
public class AppUser : IdentityUser
{
   public IEnumerable<IdentityRole>? Roles { get; set; }
}