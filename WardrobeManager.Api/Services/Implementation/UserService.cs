#region

using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WardrobeManager.Api.Database;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Repositories;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;

#endregion

namespace WardrobeManager.Api.Services.Implementation;

public class UserService : IUserService 
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly IGenericRepository<User> _genericRepository;

    public UserService(IGenericRepository<User> genericRepository, UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _genericRepository = genericRepository;
    }

    public async Task<User?> GetUser(int id)
    {
        return await _genericRepository.GetAsync(id);
    }

    public async Task<bool> DoesUserExist(int id)
    {
        if (await _genericRepository.GetAsync(id) != null)
        {
            return true;
        }

        return false;
    }

    // Middleware does this. We don't allow an existing user to 'Add' or 'Create' a user
    // Users are only created based off of an authenticated user's login info
    public async Task CreateUser()
    {
        var newUser = new User();
        newUser.ServerClothingItems = new List<ServerClothingItem>();

        var sampleClothingItems = new List<ServerClothingItem>
        {
            // New GUID are provided. They do not have a matching image as other guids would. The aim is to make our ImageEndpoint return a 'missing photo' image 
            new ServerClothingItem("Example T-Shirt", ClothingCategory.TShirt, Season.Fall, WearLocation.HomeAndOutside,
                false, 5, null),
            new ServerClothingItem("Example Pants", ClothingCategory.Jeans, Season.SummerAndFall,
                WearLocation.HomeAndOutside, false, 20, null),
        };
        newUser.ServerClothingItems = sampleClothingItems;
        
        await _genericRepository.CreateAsync(newUser);
        await _genericRepository.SaveAsync();   
    }

    public async Task UpdateUser(int userId, EditedUserDTO editedUser)
    {
        var dbRecord = await GetUser(userId);
        Debug.Assert(dbRecord != null, "At this point in the pipeline user should be created");

        dbRecord.Name = editedUser.Name;
        // Not validating the base64. If its invalid the browser can complain
        dbRecord.ProfilePictureBase64 = editedUser.ProfilePictureBase64;
        await _genericRepository.SaveAsync();
    }

    public async Task DeleteUser(int userId)
    {
        var dbRecord = await GetUser(userId);
        Debug.Assert(dbRecord != null, "At this point in the pipeline user should be created");

        _genericRepository.Remove(dbRecord);
        await _genericRepository.SaveAsync();
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
            return (false, "Admin user already exists!");
        }

        var hasher = new PasswordHasher<User>();

        var adminRoleExists = await _roleManager.RoleExistsAsync("Admin");
        Debug.Assert(adminRoleExists == true, "Admin role should exist (created in db init)!");

        var user = new User
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

        await _genericRepository.SaveAsync();

        return (true, "Admin user created!");
    }
}