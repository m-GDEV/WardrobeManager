#region

using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Shared.DTOs;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;

#endregion

namespace WardrobeManager.Api.Services.Interfaces;
public interface IClothingService
{
    Task<List<ClothingItemDTO>?> GetAllClothingAsync(string userId);
    
    Task<ClothingItemDTO?> GetClothingItemAsync(string userId, int itemId);
    Task AddNewClothingItem(string userId, NewClothingItemDTO newNewClothingItem);
    Task DeleteClothingItem(string userId, int itemId);
}
