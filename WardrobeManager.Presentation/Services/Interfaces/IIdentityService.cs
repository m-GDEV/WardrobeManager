using System.Security.Claims;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Presentation.Services.Interfaces;

public interface IIdentityService
{
    Task<bool> SignupAsync(AuthenticationCredentialsModel credentials);
    Task<bool> LoginAsync(AuthenticationCredentialsModel credentials);
    Task<bool> LogoutAsync();
    Task<bool> IsAuthenticated();
    Task<ClaimsPrincipal> GetUserInformation();
}