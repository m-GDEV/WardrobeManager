#region

using WardrobeManager.Shared.Enums;

#endregion

namespace WardrobeManager.Shared.Models;

/// <summary> DTO that client sends to create new clothing item </summary>
public class NewOrEditedClothingItemDTO
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public NewOrEditedClothingItemDTO() { } // ONLY FOR DESERIALIZER. THIS SHIT BETTER HAVE NO REFERENCES
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


    public NewOrEditedClothingItemDTO
        (
         string name, ClothingCategory category, Season season, bool favourited,
         WearLocation wearLocation, int desiredTimesWornBeforeWash, string imageBase64
        ) {
            this.Name = name;
            this.Category = category;
            this.Season = season;
            this.Favourited = favourited;
            this.WearLocation = wearLocation;
            this.DesiredTimesWornBeforeWash = desiredTimesWornBeforeWash;
            this.ImageBase64 = imageBase64;
        }

    public string Name { get; set; }
    public ClothingCategory Category { get; set; }
    public Season Season { get; set; }
    public bool Favourited { get; set; }
    public WearLocation WearLocation { get; set; }
    public int DesiredTimesWornBeforeWash { get; set; }
    public string ImageBase64 { get; set; }
}
