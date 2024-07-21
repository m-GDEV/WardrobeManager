using WardrobeManager.Shared.Models;

namespace WardrobeManager.Presentation.Services.Interfaces;
public interface IApiService
{
    ValueTask DisposeAsync();
    Task<List<ServerClothingItem>?> GetClothing();
    Task Add(NewOrEditedClothingItem clothing);
    Task Delete(ServerClothingItem clothing);
    Task Update(NewOrEditedClothingItem clothing);
    Task<bool> IsUserInitialized();
    Task CreateUser();
}
