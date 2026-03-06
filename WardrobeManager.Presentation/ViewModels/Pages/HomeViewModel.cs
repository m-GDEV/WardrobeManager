using Blazing.Mvvm.ComponentModel;

namespace WardrobeManager.Presentation.ViewModels.Pages;

[ViewModelDefinition(Lifetime = ServiceLifetime.Scoped)]
public partial class HomeViewModel(
    )
    : ViewModelBase
{
}