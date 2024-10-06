using WardrobeManager.Shared.Models;

namespace WardrobeManager.Presentation.Services.Interfaces;
public interface IApiService
{
    ValueTask DisposeAsync();

    // Things to do with new/edited items
    Task Add(NewOrEditedClothingItemDTO clothing);
    Task Update(NewOrEditedClothingItemDTO clothing);
    
    // Things to do with existing items
    Task<List<ServerClothingItem>?> GetClothing();
    Task<List<ServerClothingItem>?> GetFilteredClothing(FilterModel model);
    Task Delete(ServerClothingItem clothing);
    Task Wear(ServerClothingItem clothing);
    Task Wash(ServerClothingItem clothing);

}
