using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WardrobeManager.Shared.Models;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Exceptions;
using WardrobeManager.Api.Database.Services.Interfaces;

namespace WardrobeManager.Api.Database.Services.Implementation;

public class UserService : IUserService
{
    private readonly DatabaseContext _context;

    public UserService(DatabaseContext databaseContext)
    {
        _context = databaseContext;
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
                new ServerClothingItem("Example T-Shirt", ClothingCategory.TShirt, Season.Fall, WearLocation.HomeAndOutside, false, 5, Guid.NewGuid()),
                new ServerClothingItem("Example Pants", ClothingCategory.Jeans, Season.SummerAndFall, WearLocation.HomeAndOutside, false, 20, Guid.NewGuid()),
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
}
