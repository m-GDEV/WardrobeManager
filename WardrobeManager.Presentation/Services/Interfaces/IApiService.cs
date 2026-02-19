#region

using WardrobeManager.Shared.DTOs;
using WardrobeManager.Shared.Models;

#endregion

namespace WardrobeManager.Presentation.Services.Interfaces;
public interface IApiService
{
    ValueTask DisposeAsync();

    // Misc 
    Task<HttpResponseMessage> CheckApiConnection();
    Task<HttpResponseMessage> AddLog(LogDTO log);
    
    // User Management
    Task<bool> DoesAdminUserExist();
    
    // bool: succeeded?, string: text description
    Task<(bool, string)> CreateAdminUserIfMissing(AdminUserCredentials credentials);
}
