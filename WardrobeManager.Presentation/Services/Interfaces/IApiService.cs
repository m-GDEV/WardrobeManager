using WardrobeManager.Shared.Models;

namespace WardrobeManager.Presentation.Services.Interfaces;
public interface IApiService
{
    ValueTask DisposeAsync();
    Task<List<ServerClothingItem>?> GetClothing();
    Task Add(NewOrEditedClothingItemDTO clothing);
    Task Delete(ServerClothingItem clothing);
    Task Update(NewOrEditedClothingItemDTO clothing);
    // Task<bool> IsUserInitialized();
    // Task CreateUser();
}
