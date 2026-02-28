#region

using WardrobeManager.Shared.DTOs;
using WardrobeManager.Shared.Models;

#endregion

namespace WardrobeManager.Presentation.Services.Interfaces;
public interface IApiService
{
    ValueTask DisposeAsync();
    
    // Clothing
    Task<List<ClothingItemDTO>> GetAllClothingItemsAsync();
    Task AddNewClothingItemAsync(NewClothingItemDTO newNewClothingItem);
    Task DeleteClothingItemAsync( int itemId);
    
    // Misc 
    Task<bool> CheckApiConnection();
    Task<HttpResponseMessage> AddLog(LogDTO log);
    
    // Onboarding
    Task<bool> DoesAdminUserExist();
    
    /// <summary>
    /// Creates an admin user if one doesn't exist (used for onboarding) 
    /// </summary>
    /// <param name="credentials">Onboarding user credentials</param>
    /// <returns>bool: succeeded, string: text description</returns>
    Task<Result<string>> CreateAdminUserIfMissing(AuthenticationCredentialsModel credentials);
}
