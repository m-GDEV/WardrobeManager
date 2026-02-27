#region

using WardrobeManager.Shared.Enums;

#endregion

namespace WardrobeManager.Shared.DTOs;

public class ClothingItemDTO
{
    public ClothingItemDTO() {}
    
    public int Id { get; set; }
    public string Name { get; set; }
    public ClothingCategory Category { get; set; }
    public Guid? ImageGuid { get; set; }
}

