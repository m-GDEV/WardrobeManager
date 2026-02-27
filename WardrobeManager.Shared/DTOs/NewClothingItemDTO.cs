#region

using WardrobeManager.Shared.Enums;

#endregion

namespace WardrobeManager.Shared.DTOs;

public class NewClothingItemDTO
{
    public NewClothingItemDTO() {}
    public NewClothingItemDTO(string name, ClothingCategory category, string imageBase64)
    {
        Name = name;
        Category = category;
        ImageBase64 = imageBase64;
    }
    
    
    public string Name { get; set; }
    public ClothingCategory Category { get; set; }
    public string? ImageBase64 { get; set; } 
}

