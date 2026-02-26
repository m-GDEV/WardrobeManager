using WardrobeManager.Api.Database.Entities;

namespace WardrobeManager.Api.Repositories;

public interface IClothingRepository : IGenericRepository<ClothingItem>
{
   Task<ClothingItem?> GetAsync(string userId, int itemId);
   Task<List<ClothingItem>> GetAllAsync(string userId);
}