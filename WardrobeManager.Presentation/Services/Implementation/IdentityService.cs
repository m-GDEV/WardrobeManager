using System.Security.Claims;
using WardrobeManager.Presentation.Identity;
using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Presentation.Services.Implementation;

public class IdentityService(IAccountManagement accountManagement, INotificationService notificationService)
    : IIdentityService
{
    public async Task<bool> SignupAsync(AuthenticationCredentialsModel credentials)
    {
        var res = await accountManagement.RegisterAsync(credentials.email, credentials.password);

        if (!res.Succeeded)
        {
            foreach (var error in res.ErrorList)
            {
                notificationService.AddNotification(error, NotificationType.Error);
            }

            return false;
        }

        return true;
    }

    public async Task<bool> LoginAsync(AuthenticationCredentialsModel credentials)
    {
        var res = await accountManagement.LoginAsync(credentials.email, credentials.password);

        if (!res.Succeeded)
        {
            foreach (var error in res.ErrorList)
            {
                notificationService.AddNotification(error, NotificationType.Error);
            }

            return false;
        }

        return true;
    }

    public async Task<bool> LogoutAsync()
    {
        var success = await accountManagement.LogoutAsync();
        if (!success)
            notificationService.AddNotification("Could not log out (are you already logged out?).",
                NotificationType.Error);
        else notificationService.AddNotification("Logged out successfully!", NotificationType.Success);

        return success;
    }

    public async Task<ClaimsPrincipal> GetUserInformation()
    {
        var res = await accountManagement.GetAuthenticationStateAsync();
        return res.User;
    }

    public async Task<bool> IsAuthenticated()
    {
        var res = await accountManagement.GetAuthenticationStateAsync();
        return res.User.Identity?.IsAuthenticated ?? false;
    }
}