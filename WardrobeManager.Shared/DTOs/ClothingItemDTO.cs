#region

using WardrobeManager.Shared.Enums;

#endregion

namespace WardrobeManager.Shared.DTOs;

public class ClothingItemDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ClothingCategory Category { get; set; }
    public Season Season { get; set; }
    public ClothingSize Size { get; set; }
    public WearLocation WearLocation { get; set; }
    public Guid? ImageGuid { get; set; }
    public bool Favourited { get; set; }
    public int TimesWornTotal { get; set; }
    public DateTime LastWorn { get; set; }
    public DateTime DateAdded { get; set; }
    public DateTime DateUpdated { get; set; }
}

