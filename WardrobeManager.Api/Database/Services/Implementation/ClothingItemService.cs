using Microsoft.EntityFrameworkCore;
using WardrobeManager.Shared.Models;
using WardrobeManager.Api.Database.Services.Interfaces;

namespace WardrobeManager.Api.Database.Services.Implementation;

public class ClothingItemService : IClothingItemService
{
    private readonly DatabaseContext _databaseContext;

    public ClothingItemService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    // public async Task CreateUser(string userId)

    public async Task<List<ServerClothingItem>> GetClothes()
    {
        return await _databaseContext.ClothingItems.ToListAsync();
    }

    public async Task Add(ServerClothingItem item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        await _databaseContext.ClothingItems.AddAsync(item);
        await _databaseContext.SaveChangesAsync();
    }

    public async Task Delete(ServerClothingItem item)
    {
        _databaseContext.ClothingItems.Remove(item);
        await _databaseContext.SaveChangesAsync();
    }
    public async Task Delete(int Id)
    {
        var item = await _databaseContext.ClothingItems.SingleOrDefaultAsync<ServerClothingItem>(s => s.Id == Id);
        _databaseContext.ClothingItems.Remove(item);
        await _databaseContext.SaveChangesAsync();
    }

    public async Task Update(ServerClothingItem item)
    {
        // throws exception if it finds more than one (good cus we serial number should be unique)
        var oldItem = await _databaseContext.ClothingItems.SingleOrDefaultAsync(s => s.Id == item.Id);

        if (oldItem != null)
        {
            await Delete(oldItem);

            await _databaseContext.ClothingItems.AddAsync(item);

            await _databaseContext.SaveChangesAsync();
        }
        else
        {
            throw new Exception("Item does not exist");
        }
    }
}
