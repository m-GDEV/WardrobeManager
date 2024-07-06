using Microsoft.EntityFrameworkCore;
using WardrobeManager.Api.Database.Models;
using WardrobeManager.Api.Database.Services.Interfaces;

namespace WardrobeManager.Api.Database.Services.Implementation;

public class ClothingItemService : IClothingItemService
{
    private readonly DatabaseContext _databaseContext;

    public ClothingItemService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<List<ClothingItem>> GetClothes()
    {
        return await _databaseContext.ClothingItems.ToListAsync();
    }

    public async Task AddClothingItem(ClothingItem item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        await _databaseContext.ClothingItems.AddAsync(item);
        await _databaseContext.SaveChangesAsync();
    }

    public async Task DeleteClothingItem(ClothingItem item)
    {
        _databaseContext.ClothingItems.Remove(item);
        await _databaseContext.SaveChangesAsync();
    }

}
