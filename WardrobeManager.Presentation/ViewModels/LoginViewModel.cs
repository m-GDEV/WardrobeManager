using Blazing.Mvvm.ComponentModel;
using Blazing.Mvvm.Components;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using WardrobeManager.Presentation.Identity;
using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Presentation.ViewModels;

[ViewModelDefinition(Lifetime = ServiceLifetime.Scoped)]
public partial class LoginViewModel(
    IAccountManagement accountManagement,
    INotificationService notificationService,
    IMvvmNavigationManager navManager,
    IIdentityService identityService
    )
    : ViewModelBase
{
    // Public Properties
    [ObservableProperty]
    private AuthenticationCredentialsModel _authenticationCredentialsModel = new();

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
            await LoginAsync();
        }
    }

    public override async Task OnInitializedAsync()
    {
        var isAuthenticated = await identityService.IsAuthenticated();
        if (isAuthenticated)
        {
            navManager.NavigateTo<DashboardViewModel>();
        }
    }

    public async Task LoginAsync()
    {
        var success = await identityService.LoginAsync(AuthenticationCredentialsModel);
        if (!success) return;
        
        navManager.NavigateTo<DashboardViewModel>();
    }
}