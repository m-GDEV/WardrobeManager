using System.Diagnostics.CodeAnalysis;
using WardrobeManager.Shared.Enums;
using System.Text.Json.Serialization;

namespace WardrobeManager.Shared.Models;

public class ServerClothingItem
{
    public ServerClothingItem() { } // ONLY FOR DESERIALIZER, DO NOT USE THIS. THIS SHIT BETTER HAVE NO REFERENCES
                                    // deserializer needs a way to create the object without any fields so it can assign them if they exist

    public ServerClothingItem(string name, ClothingCategory category, Guid? imageGuid)
    {
        // this.UserId = userId;
        this.Name = name;
        this.Category = category;
        this.ImageGuid = imageGuid;
    }

    // EF Core modifies
    public int Id { get; set; }

    // represents a mandatory one-to-many relationship with a User as
    // the following 2 fields are not nullable
    // https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-many
    [JsonIgnore] // causes the serializer to run into loop
        public User User { get; set; } // navigation property
    public int UserId { get; set; }

    // User modifies
    public string Name { get; set; }
    public ClothingCategory Category { get; set; }
    public Season? Season { get; set; } // doesn't have to be set
    public Guid? ImageGuid { get; set; } // modified in spirit by the user, they change the image. program assigns guid
    public bool Favourited { get; set; } = false;

    // # of times the user wants to wear a piece of clothing before washing it
    // will be used to show something like '2 wears remaining' or 'worn 2 times extra'
    // this will be determine using this var and TimesWornSinceWash
    public int DesiredTimesWornBeforeWash { get; set; } = 0;


    // Only program modifies
    public int TimesWornSinceWash { get; set; } = 0;
    public int TimesWornTotal { get; set; } = 0; // initialized at zero since the user can change this later
    public DateTime LastWorn { get; set; }
    public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    public DateTime DateUpdated { get; set; } = DateTime.UtcNow; // will be updated directly. at init this is fine for default

    public void Wash()
    {
        TimesWornSinceWash = 0;
    }
    public void Wear()
    {
        TimesWornTotal += 1;
        LastWorn = DateTime.UtcNow;
    }
}

