#region

using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;

#endregion

namespace WardrobeManager.Api.Services.Interfaces;
public interface IClothingItemService
{
    // ---- Methods for multiple clothing items ---
    Task<List<ServerClothingItem>?> GetAllClothing(string userId);
    Task<List<ServerClothingItem>?> GetFilteredClothing(string userId, FilterModel model);

    // ---- Methods for one clothing item ---
    Task<ServerClothingItem?> GetClothingItem(string userId, int itemId);
    Task AddClothingItem(string userId, NewOrEditedClothingItemDTO newItem);
    Task UpdateClothingItem(string userId, int itemId, NewOrEditedClothingItemDTO editedItem);
    Task DeleteClothingItem(string userId, int itemId);
    Task CallMethodOnClothingItem(string userId, int itemId, ActionType type);
}
