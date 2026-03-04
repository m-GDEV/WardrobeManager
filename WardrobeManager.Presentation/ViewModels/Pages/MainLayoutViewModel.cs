using Blazing.Mvvm.ComponentModel;
using Blazing.Mvvm.Components;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components.Web;
using Sysinfocus.AspNetCore.Components;
using WardrobeManager.Presentation.Services.Interfaces;

namespace WardrobeManager.Presentation.ViewModels.Pages;

[ViewModelDefinition(Lifetime = ServiceLifetime.Scoped)]
public partial class MainLayoutViewModel(
    IMvvmNavigationManager navManager,
    IApiService apiService,
    INotificationService _notificationService,
    Initialization sysinfocusComponentsinit,
    BrowserExtensions browserExtensions
)
    : ViewModelBase
{
    [ObservableProperty] public ErrorBoundary? _errorBoundary;

    public void HandleMainLayoutClickEvent()
    {
        sysinfocusComponentsinit.HandleMainLayoutClickEvent();
    }

    // This runs every time any page is (fully) reloaded (in the browser)
    public override async Task OnInitializedAsync()
    {
        var res = await apiService.DoesAdminUserExist();
        if (res is false)
        {
            navManager.NavigateTo<OnboardingViewModel>();
        }

        await base.OnInitializedAsync();
    }

    public override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await browserExtensions.SetToLocalStorage("theme", "dark");
            await sysinfocusComponentsinit.InitializeTheme();
        }
    }

    public override void OnParametersSet()
    {
        ErrorBoundary?.Recover();
    }
}