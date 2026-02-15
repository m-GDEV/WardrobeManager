#region

using WardrobeManager.Shared.Enums;

#endregion

namespace WardrobeManager.Shared.Models;

public class ClientClothingItem(string name, ClothingCategory category, string ImageBase64)
{
    public int Id { get; set; }
    public string Name { get; set; } = name;
    public ClothingCategory Category { get; set; } = category;
    public string ImageBase64 { get; set; } = ImageBase64;
}

