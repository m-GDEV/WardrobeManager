#region

using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WardrobeManager.Api.Database;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Repositories;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.DTOs;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;

#endregion

namespace WardrobeManager.Api.Services.Implementation;

public class UserService : IUserService 
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;

    public UserService(UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<User?> GetUser(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public async Task<bool> DoesUserExist(string userId)
    {
        if (await GetUser(userId) != null)
        {
            return true;
        }

        return false;
    }

    // Middleware does this. We don't allow an existing user to 'Add' or 'Create' a user
    // Users are only created based off of an authenticated user's login info
    public async Task CreateUser()
    {
        var newUser = new User
        {
            ServerClothingItems = new List<ClothingItem>
            {
                // New GUID are provided. They do not have a matching image as other guids would. The aim is to make our ImageEndpoint return a 'missing photo' image 
                new ClothingItem("Example T-Shirt", ClothingCategory.TShirt, Season.Fall,
                    WearLocation.HomeAndOutside,
                    false, 5, null),
                new ClothingItem("Example Pants", ClothingCategory.Jeans, Season.SummerAndFall,
                    WearLocation.HomeAndOutside, false, 20, null),
            }
        };
        
        await _userManager.CreateAsync(newUser);
    }

    public async Task UpdateUser(string userId, EditedUserDTO editedUser)
    {
        var dbRecord = await GetUser(userId);
        Debug.Assert(dbRecord != null, "At this point in the pipeline user should be created");

        dbRecord.Name = editedUser.Name;
        // Not validating the base64. If its invalid the browser can complain
        dbRecord.ProfilePictureBase64 = editedUser.ProfilePictureBase64;
        await _userManager.UpdateAsync(dbRecord);
    }

    public async Task DeleteUser(string userId)
    {
        var dbRecord = await GetUser(userId);
        Debug.Assert(dbRecord != null, "At this point in the pipeline user should be created");

        await _userManager.DeleteAsync(dbRecord);
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

    public async Task<(bool, string)> CreateAdminIfMissing(AuthenticationCredentialsModel credentials)
    {
        if (await DoesAdminUserExist())
        {
            return (false, "Admin user already exists!");
        }

        var adminRoleExists = await _roleManager.RoleExistsAsync("Admin");
        Debug.Assert(adminRoleExists == true, "Admin role should exist (created in db init)!");

        var user = new User
        {
            Email = credentials.Email,
            NormalizedEmail =  credentials.Email.ToUpper(),
            UserName =  credentials.Email.ToUpper(),
            NormalizedUserName =  credentials.Email.ToUpper(),
        };

        var createResult = await _userManager.CreateAsync(user, credentials.Password);

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

        return (true, "Admin user created!");
    }
}