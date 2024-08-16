using WardrobeManager.Shared.Models;
using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Api.Database.Services.Interfaces;
public interface IClothingItemService
{
    // ---- Methods for multiple clothing items ---
    Task<List<ServerClothingItem>?> GetAllClothing(int userId);

    // ---- Methods for one clothing item ---
    Task<ServerClothingItem?> GetClothingItem(int userId, int itemId);
    Task AddClothingItem(int userId, NewOrEditedClothingItemDTO newItem);
    Task UpdateClothingItem(int userId, int itemId, NewOrEditedClothingItemDTO editedItem);
    Task DeleteClothingItem(int userId, int itemId);
    Task CallMethodOnClothingItem(int userId, int itemId, ActionType type);
}
