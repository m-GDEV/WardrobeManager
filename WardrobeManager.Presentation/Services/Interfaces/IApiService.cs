using WardrobeManager.Shared.Models;

namespace WardrobeManager.Presentation.Services.Interfaces;
public interface IApiService
{
    ValueTask DisposeAsync();
    Task<List<ServerClothingItem>?> GetClothing();
    Task Add(ClientClothingItem clothing);
    Task Delete(ServerClothingItem clothing);
    Task Update(ClientClothingItem clothing);


}