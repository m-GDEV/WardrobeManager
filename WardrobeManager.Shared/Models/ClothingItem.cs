using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Shared.Models;

public class ClothingItem(string name, ClothingCategory category, Guid? imageGuid)
{
    public int Id { get; set; }
    public string Name { get; set; } = name;
    public ClothingCategory Category { get; set; } = category;
    public Guid? ImageGuid { get; set; } = imageGuid;
}

