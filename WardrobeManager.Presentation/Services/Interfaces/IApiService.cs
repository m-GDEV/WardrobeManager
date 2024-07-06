using WardrobeManager.Shared.Models;

namespace WardrobeManager.Presentation.Services.Interfaces;
public interface IApiService
{
    ValueTask DisposeAsync();
    Task<List<ClothingItem>?> GetClothing();
}