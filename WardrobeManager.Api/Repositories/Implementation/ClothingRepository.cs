using Microsoft.EntityFrameworkCore;
using WardrobeManager.Api.Database;
using WardrobeManager.Api.Database.Entities;

namespace WardrobeManager.Api.Repositories;

public class ClothingRepository: GenericRepository<ClothingItem>, IClothingRepository
{
    public ClothingRepository(DatabaseContext context, DbSet<ClothingItem> clothingItems) : base(context)
    {
    }

    public async Task<ClothingItem?> GetAsync(string userId, int itemId)
    {
        return await _dbSet.FirstOrDefaultAsync(item => item.Id == itemId && item.UserId == userId);
    }
    public async Task<List<ClothingItem>> GetAllAsync(string userId)
    {
        return await _dbSet.Where(x => x.UserId == userId).ToListAsync();
    }
}