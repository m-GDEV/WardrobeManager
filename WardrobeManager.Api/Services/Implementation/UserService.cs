#region

using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WardrobeManager.Api.Database;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;

#endregion

namespace WardrobeManager.Api.Services.Implementation;

public class UserService : IUserService
{
    private readonly DatabaseContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserService(DatabaseContext databaseContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _context = databaseContext;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // Auth0Id methods (used in middleware)
    public async Task<User?> GetUser(string Auth0Id)
    {
        var user = await _context.Users.SingleOrDefaultAsync(user => user.Auth0Id == Auth0Id);

        return user;
    }

    public async Task<bool> DoesUserExist(string Auth0Id)
    {
        if (await _context.Users.AnyAsync(user => user.Auth0Id == Auth0Id))
        {
            return true;
        }
        return false;
    }

    // Middleware does this. We don't allow an existing user to 'Add' or 'Create' a user
    // Users are only created based off of an authenticated user's login info
    public async Task CreateUser(string Auth0Id)
    {
        // fail silently if user exists
        if (!_context.Users.Any(s => s.Auth0Id == Auth0Id))
        {
            var newUser = new User(Auth0Id);
            newUser.ServerClothingItems = new List<ServerClothingItem>();

            var sampleClothingItems = new List<ServerClothingItem>
            {
                // New GUID are provided. They do not have a matching image as other guids would. The aim is to make our ImageEndpoint return a 'missing photo' image 
                new ServerClothingItem("Example T-Shirt", ClothingCategory.TShirt, Season.Fall, WearLocation.HomeAndOutside, false, 5, null),
                new ServerClothingItem("Example Pants", ClothingCategory.Jeans, Season.SummerAndFall, WearLocation.HomeAndOutside, false, 20, null),
            };
            newUser.ServerClothingItems = sampleClothingItems;

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
        }
    }

    // Normal methods for endpoints
    public async Task<User?> GetUser(int userId)
    {
        var user = await _context.Users.FindAsync(userId);

        return user;
    }

    public async Task UpdateUser(int userId, EditedUserDTO editedUser)
    {
        var dbRecord = await GetUser(userId);
        Debug.Assert(dbRecord != null, "At this point in the pipeline user should be created");

        dbRecord.Name = editedUser.Name;
        // Not validating the base64. If its invalid the browser can complain
        dbRecord.ProfilePictureBase64 = editedUser.ProfilePictureBase64;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUser(int userId)
    {
        var dbRecord = await GetUser(userId);
        Debug.Assert(dbRecord != null, "At this point in the pipeline user should be created");

        _context.Users.Remove(dbRecord);
        await _context.SaveChangesAsync();
    }

    // These methods are called by the frontend during the onboarding process
    
    // REVIEW: performance can likely be improved
    public async Task<bool> DoesAdminUserExist()
    {
        var users = await _userManager.Users.ToListAsync();
        
        var adminRoleExists = await _roleManager.RoleExistsAsync("Admin");
        Debug.Assert(adminRoleExists == true, "Admin role should exist (created in db init)!");

        foreach (var user in users)
        {
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return true;
            }   
        }

        return false;
    }

    public async Task<(bool, string)> CreateAdminIfMissing(string email, string password)
    {
        if (await DoesAdminUserExist())
        {
            return (false,"Admin user already exists!");
        }
        
        var hasher = new PasswordHasher<AppUser>();
            
        var adminRoleExists = await _roleManager.RoleExistsAsync("Admin");
        Debug.Assert(adminRoleExists == true, "Admin role should exist (created in db init)!");

        var user = new AppUser
        {
            Email = email,
            NormalizedEmail = email.ToUpper(), 
            UserName = email.ToUpper(),
            NormalizedUserName = email.ToUpper(), 
        };

        var hashed = hasher.HashPassword(user, password);
        user.PasswordHash = hashed;
        var createResult = await _userManager.CreateAsync(user);

        if (createResult.Succeeded is false)
        {
            var errors = createResult.Errors.Select(e => e.Description).ToList();
            return (false, string.Join(" ", errors));
        }
        
        var roleResult = await _userManager.AddToRoleAsync(user, "Admin");

        if (roleResult.Succeeded is false)
        {
            var errors = roleResult.Errors.Select(e => e.Description).ToList();
            return (false, string.Join(" ", errors));
        }
            
        await _context.SaveChangesAsync();

        return (true, "Admin user created!");
    }
}
