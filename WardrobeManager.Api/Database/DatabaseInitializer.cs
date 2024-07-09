using WardrobeManager.Shared.Models;
using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Api.Database;

public class DatabaseInitializer
{
    public static void Initialize(DatabaseContext context)
    {
        if (context.ClothingItems.Count() > 0) { return; }


        var clothingItems = new List<ServerClothingItem>
        {
            // Creating two base clothing items
            new ServerClothingItem("Test Clothing 1", ClothingCategory.TShirt, null),
            new ServerClothingItem("Test Clothing 2", ClothingCategory.Sweatpants, null)
        };


        clothingItems.ForEach(d => context.ClothingItems.Add(d));
        context.SaveChanges();
    }
}
