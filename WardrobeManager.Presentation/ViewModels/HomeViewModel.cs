using Blazing.Mvvm.ComponentModel;
using Blazing.Mvvm.Components;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using WardrobeManager.Presentation.Identity;
using WardrobeManager.Presentation.Pages.Public;
using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Presentation.ViewModels;

[ViewModelDefinition(Lifetime = ServiceLifetime.Scoped)]
public partial class HomeViewModel(
    INotificationService notificationService,
    NavigationManager navigationManager,
    IMvvmNavigationManager navManager,
    
    IIdentityService identityService
    )
    : ViewModelBase
{
}