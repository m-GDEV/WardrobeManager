using Microsoft.EntityFrameworkCore;
using WardrobeManager.Shared.Models;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Exceptions;
using WardrobeManager.Api.Database.Services.Interfaces;

namespace WardrobeManager.Api.Database.Services.Implementation;

public class UserService : IUserService
{
    private readonly DatabaseContext context;

    public UserService(DatabaseContext databaseContext)
    {
        context = databaseContext;
    }

    public async Task CreateUser(string Auth0Id) {
        // fail silently if user exists
        if (!context.Users.Any(s => s.Auth0Id == Auth0Id)) {
            var newUser = new User(Auth0Id);
            newUser.ServerClothingItems = new List<ServerClothingItem>();

            var sampleClothingItems = new List<ServerClothingItem>
            {
                new ServerClothingItem("Example T-Shirt", ClothingCategory.TShirt, Season.Fall,  null),
                    new ServerClothingItem("Example Pants", ClothingCategory.Jeans, Season.Winter, null)
            };
            newUser.ServerClothingItems = sampleClothingItems;

            await context.Users.AddAsync(newUser);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> DoesUserExist(string Auth0Id) {
        if (await context.Users.AnyAsync(user => user.Auth0Id == Auth0Id)) {
            return true;
        }
        return false;
    }

    public async Task<User?> GetUser(string Auth0Id) {
        var user = await context.Users.SingleOrDefaultAsync(user => user.Auth0Id == Auth0Id);

        return user;
    }

    public async Task<User?> GetUserWithClothingItems(string Auth0Id) {
        var user = await context.Users.Include(x => x.ServerClothingItems).SingleOrDefaultAsync(user => user.Auth0Id == Auth0Id);

        return user;
    }


}
