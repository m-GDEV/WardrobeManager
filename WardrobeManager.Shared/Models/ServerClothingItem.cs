using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Shared.Models;

public class ServerClothingItem
{
    public ServerClothingItem() { } // ONLY FOR DESERIALIZER, DO NOT USE THIS. THIS SHIT BETTER HAVE NO REFERENCES
    // deserializer needs a way to create the object without any fields so it can assign them if they exist

    public ServerClothingItem(string name, ClothingCategory category, Guid? imageGuid)
    {
        this.Name = name;
        this.Category = category;
        this.ImageGuid = imageGuid;
    }

    public int Id { get; set; }
    public string Name { get; set; } 
    public ClothingCategory Category { get; set; }
    public Guid? ImageGuid { get; set; }

    // extra
    public bool Favourited { get; set; } = false;
    public int TimesWornSinceWash { get; set; } = 0;

    // # of times the user wants to wear a piece of clothing before washing it
    // will be used to show something like '2 wears remaining' or 'worn 2 times extra'
    // this will be determine using this var and TimesWornSinceWash
    public int DesiredTimesWornBeforeWash { get; set; } = 0;
    public int TimesWornTotal { get; set; } = 0; // initialized at zero since the user can change this later


    public void Wash()
    {
        TimesWornSinceWash = 0;
    }
    public void Wear()
    {
        TimesWornTotal += 1;
    }
}

