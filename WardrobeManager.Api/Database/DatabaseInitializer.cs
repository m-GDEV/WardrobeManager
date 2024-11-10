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

        // User Creation
        foreach (var user in seedUsers)
        {
            var hashed = password.HashPassword(user, "password123");
            user.PasswordHash = hashed;
            await userStore.CreateAsync(user);

            if (user.Email is not null)
            {
                var appUser = await userManager.FindByEmailAsync(user.Email);

                if (appUser is not null && user.RoleList is not null)
                {
                    await userManager.AddToRolesAsync(appUser, user.RoleList);
                }
            }
        }

    }

    private class SeedUser : AppUser
    {
        public string[]? RoleList { get; set; }
    }
    private static readonly IEnumerable<SeedUser> seedUsers =
    [
        new SeedUser()
        {
            Email = "leela@contoso.com", 
            NormalizedEmail = "LEELA@CONTOSO.COM", 
            NormalizedUserName = "LEELA@CONTOSO.COM", 
            RoleList = [ "Admin", "User" ], 
            UserName = "leela@contoso.com"
        },
        new SeedUser()
        {
            Email = "harry@contoso.com",
            NormalizedEmail = "HARRY@CONTOSO.COM",
            NormalizedUserName = "HARRY@CONTOSO.COM",
            RoleList = [ "User" ],
            UserName = "harry@contoso.com"
        },
    ];
}