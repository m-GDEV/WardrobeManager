using Blazing.Mvvm.ComponentModel;
using Blazing.Mvvm.Components;
using CommunityToolkit.Mvvm.ComponentModel;
using Sysinfocus.AspNetCore.Components;
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
    [ObservableProperty] public bool _isLoading = true;

    public List<Breadcrumb.BreadcrumbModel> breadcrumbItems;

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
        if (Item == null)
        {
            notificationService.AddNotification("Clothing item not found!", NotificationType.Error);
            navManager.NavigateTo<WardrobeViewModel>();
            return;
        }

        breadcrumbItems =
        [
            new("Wardrobe") { Url = "/wardrobe" },
            new($"Clothing Item: {Item.Name}") { Url = "" }
        ];
        IsLoading = false;
    }
}