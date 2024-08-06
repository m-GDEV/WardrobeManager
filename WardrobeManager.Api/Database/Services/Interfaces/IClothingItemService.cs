using WardrobeManager.Shared.Models;

namespace WardrobeManager.Api.Database.Services.Interfaces;
public interface IClothingItemService
{
    Task Add(ServerClothingItem item);
    Task Delete(ServerClothingItem item);
    // Task<List<ServerClothingItem>> GetClothes();
    Task Delete(int Id);
    Task Update(ServerClothingItem item);

}
