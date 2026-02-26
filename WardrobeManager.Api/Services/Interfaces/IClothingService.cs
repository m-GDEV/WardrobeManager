#region

using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;

#endregion

namespace WardrobeManager.Api.Services.Interfaces;
public interface IClothingService
{
    Task<List<ClothingItem>?> GetAllClothingAsync(string userId);
    
    Task<ClothingItem?> GetClothingItemAsync(string userId, int itemId);
}
