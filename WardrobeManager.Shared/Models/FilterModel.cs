#region

using WardrobeManager.Shared.Enums;

#endregion

namespace WardrobeManager.Shared.Models;

public class FilterModel
{
    public FilterModel() { }

    public string SearchQuery { get; set; } = string.Empty;

    public SortByCategories SortBy { get; set; } = SortByCategories.None;
    public bool IsAscending { get; set; } = true;

    public bool HasImage { get; set; }
    public bool Favourited { get; set; }
    public bool RecentlyAdded { get; set; }
    // Ensures backed doesn't filter based on one of these enums by default
    public ClothingCategory Category { get; set; } = ClothingCategory.None;
    public Season Season { get; set; } = Season.None;
    public DateTime? DateAddedFrom { get; set; } = null;
    public DateTime? DateAddedTo { get; set; } = null;
    public DateTime? LastWornFrom { get; set; } = null;
    public DateTime? LastWornTo { get; set; } = null;
    public DateTime? LastEditedFrom { get; set; } = null;
    public DateTime? LastEditedTo { get; set; } = null;
    public int TimesWorn { get; set; }
    public int TimesWornSinceWash { get; set; }
}
