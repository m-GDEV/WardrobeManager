using Microsoft.EntityFrameworkCore;
using WardrobeManager.Api.Database;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Repositories.Interfaces;

namespace WardrobeManager.Api.Repositories.Implementation;

public class ClothingRepository(DatabaseContext context) : GenericRepository<ClothingItem>(context), IClothingRepository
{
    public async Task<ClothingItem?> GetAsync(string userId, int itemId)
    {
        return await _dbSet.FirstOrDefaultAsync(item => item.Id == itemId && item.UserId == userId);
    }
    public async Task<List<ClothingItem>> GetAllAsync(string userId)
    {
        return await _dbSet.Where(x => x.UserId == userId).ToListAsync();
    }
}