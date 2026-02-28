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
    IApiService apiService,
    INotificationService notificationService
)
    : ViewModelBase
{
    // Public Properties
    [ObservableProperty] private List<ClothingItemDTO>? _clothingItems;

    [ObservableProperty]
    private Dictionary<int, ShowActionDialog> _actionDialogStates = new Dictionary<int, ShowActionDialog>();

    public override async Task OnInitializedAsync()
    {
        await FetchItemAndUpdate();
    }

    public async Task FetchItemAndUpdate()
    {
        ClothingItems = await apiService.GetAllClothingItemsAsync();
        // Reset dialogs states (in case items are removed, they wont exist in this anymore)
        ActionDialogStates = new Dictionary<int, ShowActionDialog>();
        foreach (var item in ClothingItems)
        {
            ActionDialogStates.Add(item.Id, new ShowActionDialog());
        }
    }

    public async Task RemoveItem(int itemId)
    {
        await apiService.DeleteClothingItemAsync(itemId);
        await FetchItemAndUpdate();
    }

    public void UpdateActionDialogState(int itemId, ActionType actionType, bool value)
    {
        if (ActionDialogStates.ContainsKey(itemId))
        {
            switch(actionType)
            {
               case ActionType.Delete:
                   ActionDialogStates[itemId].ShowDelete = value;
                   break;
               case ActionType.Edit:
                   ActionDialogStates[itemId].ShowEdit = value;
                   break;
               default:
                   notificationService.AddNotification("Action type not recognized!", NotificationType.Warning);
                   break;
            }
        }
        else
        {
            notificationService.AddNotification("Cannot change dialog state for not existing item!", NotificationType.Error);
        }
    }

    public bool GetActionStateSafely(int itemId, ActionType actionType)
    {
        if (ActionDialogStates.ContainsKey(itemId))
        {
            switch (actionType)
            {
                case ActionType.Delete:
                    return ActionDialogStates[itemId].ShowDelete;
                case ActionType.Edit:
                    return ActionDialogStates[itemId].ShowEdit;
            }
        }
        return false;
    }
}

public record ShowActionDialog
{
    public bool ShowDelete = false;
    public bool ShowEdit = false;
}

public enum ActionType
{
    Delete,
    Edit,
}