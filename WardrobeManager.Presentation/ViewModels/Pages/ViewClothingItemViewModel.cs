using Blazing.Mvvm.ComponentModel;
using Blazing.Mvvm.Components;
using CommunityToolkit.Mvvm.ComponentModel;
using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Shared.DTOs;
using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Presentation.ViewModels.Pages;

[ViewModelDefinition(Lifetime = ServiceLifetime.Scoped)]
public partial class ViewClothingItemViewModel(
    INotificationService notificationService,
    IMvvmNavigationManager navManager,
    IApiService apiService
)
    : ViewModelBase
{
    [ViewParameter] // gets this from the razor component from the slug
    public string? ViewItemId { get; set; }

    [ObservableProperty] public int _itemId;
    [ObservableProperty] public ClothingItemDTO? _item;

    public override async Task OnInitializedAsync()
    {
        if (ViewItemId == null)
        {
            notificationService.AddNotification("Clothing item Id cannot be null!", NotificationType.Error);
            // navManager.NavigateTo<WardrobeViewModel>();
        }
        ItemId = Convert.ToInt32(ViewItemId);
        Console.WriteLine($"Item id = {ItemId} | View item id = {ViewItemId}");
        Item = await apiService.GetClothingItemsAsync(ItemId); 
    }
}