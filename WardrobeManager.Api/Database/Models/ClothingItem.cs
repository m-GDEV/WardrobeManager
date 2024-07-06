using WardrobeManager.Api.Database.Enums;

namespace WardrobeManager.Api.Database.Models;

public class ClothingItem(string name, ClothingCategory category, Guid? imageGuid)
{
    public int Id { get; set; }
    public string Name { get; set; } = name;
    public ClothingCategory Category { get; set; } = category;
    public Guid? ImageGuid { get; set; } = imageGuid;
}

