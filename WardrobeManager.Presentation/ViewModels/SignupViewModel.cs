using Blazing.Mvvm.ComponentModel;
using Blazing.Mvvm.Components;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using WardrobeManager.Presentation.Identity;
using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;
using WardrobeManager.Shared.StaticResources;

namespace WardrobeManager.Presentation.ViewModels;

[ViewModelDefinition(Lifetime = ServiceLifetime.Scoped)]
public partial class SignupViewModel(
    INotificationService notificationService,
    IMvvmNavigationManager navManager,
    IIdentityService identityService
)
    : ViewModelBase
{
    // Public Properties
    [ObservableProperty]
    private AuthenticationCredentialsModel _authenticationCredentialsModel = new();

    public EditForm? EditForm { get; set; }

    // Public Methods
    // Stupid that i'm doing this but its the easiest solution and idk what the best method is
    public void SetEmail(string email)
    {
        AuthenticationCredentialsModel.Email = email;
    }

    public void SetPassword(string password)
    {
        AuthenticationCredentialsModel.Password = password;
    }

    public async Task DetectEnterPressed(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await SignupAsync();
        }
    }

    public async Task SignupAsync()
    {
        var res = StaticValidators.Validate(AuthenticationCredentialsModel);
        if (!res.Success)
        {
            notificationService.AddNotification(res.Message, NotificationType.Error);
        }
        
        var signupSuccess = await identityService.SignupAsync(AuthenticationCredentialsModel);
        if (!signupSuccess) return;
        
        // Automatically log the user in after they create an account
        var loginSuccess = await identityService.LoginAsync(AuthenticationCredentialsModel);
        if (!loginSuccess)
        {
            notificationService.AddNotification("Account created, but could not log you in. Please try again.", NotificationType.Error);
        }
        
        notificationService.AddNotification("Account created and logged in successfully!", NotificationType.Success);
        navManager.NavigateTo<DashboardViewModel>();
    }
}