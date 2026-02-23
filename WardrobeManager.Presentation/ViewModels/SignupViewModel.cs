using Blazing.Mvvm.ComponentModel;
using Blazing.Mvvm.Components;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components.Forms;
using WardrobeManager.Presentation.Identity;
using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Presentation.ViewModels;

[ViewModelDefinition(Lifetime = ServiceLifetime.Scoped)]
public partial class SignupViewModel : ViewModelBase
{
    private readonly IAccountManagement _accountManagement;
    private readonly INotificationService _notificationService;
    private readonly IMvvmNavigationManager  _navManager;

    public SignupViewModel(IAccountManagement accountManagement, INotificationService notificationService,  IMvvmNavigationManager navManager)
    {
        _accountManagement = accountManagement;
        _notificationService = notificationService;
        _navManager = navManager;
    }
    
    
    // Public Properties
    [ObservableProperty]
    private AuthenticationCredentialsModel _authenticationCredentialsModel = new AuthenticationCredentialsModel();
    public EditForm? EditForm { get; set; }
    
    // Public Methods
    // Stupid that i'm doing this but its the easiest solution and idk what the best method is
    public void SetEmail(string email)
    {
        AuthenticationCredentialsModel.email = email;
    }
    public void SetPassword(string password)
    {
        AuthenticationCredentialsModel.password = password;
    }
    
    public async Task LoginAsync()
    {
        var res = await _accountManagement.RegisterAsync(_authenticationCredentialsModel.email, _authenticationCredentialsModel.password);

        if (!res.Succeeded)
        {
            foreach (var error in res.ErrorList)
            {
                _notificationService.AddNotification(error, NotificationType.Error);
            }

            return;
        }
        
        // The way the current Auth system works the user is not automatically signed in after making an account
        _notificationService.AddNotification("Account Created, please log in", NotificationType.Success);
        _navManager.NavigateTo("/login");
    }
}