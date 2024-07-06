using WardrobeManager.Shared.Models;

namespace WardrobeManager.Api.Database.Services.Interfaces;
public interface IClothingItemService
{
    Task AddClothingItem(ClothingItem item);
    Task DeleteClothingItem(ClothingItem item);
    Task<List<ClothingItem>> GetClothes();
}