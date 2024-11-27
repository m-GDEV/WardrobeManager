using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WardrobeManager.Shared.Models;
using WardrobeManager.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using WardrobeManager.Api.Database.Models;

namespace WardrobeManager.Api.Database;

public class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceScope scope)
    {
        // Get stuff
        using var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        
        // Create db and applies migrations (useful when running on new environment, don't need to do this manually)
        // Like called 'Update-Database' I think
        await context.Database.MigrateAsync();

        if (context.AppUsers.Any())
        {
            return;
        }

        var userStore = new UserStore<AppUser>(context);
        var password = new PasswordHasher<AppUser>();

        // Role Creation
        string[] roles = ["Admin", "User"];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

    }
}