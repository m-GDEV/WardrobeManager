#region

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WardrobeManager.Api.Database.Entities;

#endregion

namespace WardrobeManager.Api.Database;

public class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceScope scope)
    {
        // Get stuff
        using var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        
        // Create db and applies migrations (useful when running on new environment, don't need to do this manually)
        // Like called 'Update-Database' I think
        await context.Database.MigrateAsync();

        if (context.Users.Any())
        {
            return;
        }

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