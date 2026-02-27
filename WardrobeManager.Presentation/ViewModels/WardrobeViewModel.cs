using Blazing.Mvvm.ComponentModel;
using Blazing.Mvvm.Components;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using WardrobeManager.Presentation.Identity;
using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Shared.DTOs;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Presentation.ViewModels;

[ViewModelDefinition(Lifetime = ServiceLifetime.Scoped)]
public partial class WardrobeViewModel(
    IMvvmNavigationManager navManager,
    IApiService apiService
)
    : ViewModelBase
{
   // Public Properties
   [ObservableProperty] private List<ClothingItemDTO> _clothingItems;

   public override async Task OnInitializedAsync()
   {
       ClothingItems = await apiService.GetAllClothingItems();
   }

   public async Task RemoveItem(int itemId)
   {
       await apiService.RemoveClothingItem(itemId);
       ClothingItems = await apiService.GetAllClothingItems();
   }
}