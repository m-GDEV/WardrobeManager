#region

using WardrobeManager.Shared.Enums;

#endregion

namespace WardrobeManager.Shared.DTOs;

public class NewClothingItemDTO
{
    public NewClothingItemDTO() {}
    public string Name { get; set; } = string.Empty;
    public ClothingCategory Category { get; set; }  = ClothingCategory.None;
    public string? ImageBase64 { get; set; } = null;
    public ClothingSize Size { get; set; }  = ClothingSize.NotSpecified;
}

