using Blazing.Mvvm.ComponentModel;
using Blazing.Mvvm.Components;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;
using WardrobeManager.Presentation.Services.Interfaces;

namespace WardrobeManager.Presentation.ViewModels;

[ViewModelDefinition(Lifetime = ServiceLifetime.Scoped)]
public partial class NavBarViewModel(
    INotificationService notificationService,
    IMvvmNavigationManager navManager,
    IIdentityService identityService,
    IApiService apiService
)
    : ViewModelBase
{
    [ObservableProperty] private bool _showUserPopover;
    [ObservableProperty] private bool _canConnectToBackend;
    [ObservableProperty] private string _usersName;

    public void ToggleUserPopover() => ShowUserPopover = !ShowUserPopover;

    public override async Task OnInitializedAsync()
    {
        var res = await apiService.CheckApiConnection();
        CanConnectToBackend = res;
        
        var user = await identityService.GetUserInformation();
        UsersName = user.Identity?.Name ?? "Logged In User";
    }

    public async Task LogoutAsync()
    {
        await identityService.LogoutAsync();
        ShowUserPopover = false;
        navManager.NavigateTo<HomeViewModel>();
    }
}