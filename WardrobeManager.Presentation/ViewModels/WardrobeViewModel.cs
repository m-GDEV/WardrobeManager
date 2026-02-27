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
   [ObservableProperty] private Dictionary<int, ShowActionDialog> _actionDialogStates;
   private bool show;

   public override async Task OnInitializedAsync()
   {
       await FetchItemAndUpdate();
   }

   public async Task FetchItemAndUpdate()
   {
       ClothingItems = await apiService.GetAllClothingItems();
       ActionDialogStates = new Dictionary<int, ShowActionDialog>();
       foreach (var item in ClothingItems)
       {
           ActionDialogStates.Add(item.Id, new ShowActionDialog());
       }
   }

   public async Task RemoveItem(int itemId)
   {
       await apiService.RemoveClothingItem(itemId);
       await FetchItemAndUpdate();
   }
}

public record ShowActionDialog
{
    public bool ShowDelete = false;
    public bool ShowEdit = false;
}