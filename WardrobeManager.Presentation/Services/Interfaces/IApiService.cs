// #region
//
// using WardrobeManager.Shared.Models;
//
// #endregion
//
// namespace WardrobeManager.Presentation.Services.Interfaces;
// public interface IApiService
// {
//     ValueTask DisposeAsync();
//
//     // Things to do with new/edited items
//     Task Add(NewOrEditedClothingItemDTO clothing);
//     Task Update(NewOrEditedClothingItemDTO clothing, int OriginalItemId);
//     
//     // Things to do with existing items
//     Task<List<ServerClothingItem>?> GetClothing();
//     Task<List<ServerClothingItem>?> GetFilteredClothing(FilterModel model);
//     Task Delete(ServerClothingItem clothing);
//     Task Wear(ServerClothingItem clothing);
//     Task Wash(ServerClothingItem clothing);
//
//     // Misc 
//     Task<HttpResponseMessage> CheckApiConnection();
//     
//     // User Management
//     Task<bool> DoesAdminUserExist();
//     
//     // bool: succeeded?, string: text description
//     Task<(bool, string)> CreateAdminUserIfMissing(AdminUserCredentials credentials);
// }
