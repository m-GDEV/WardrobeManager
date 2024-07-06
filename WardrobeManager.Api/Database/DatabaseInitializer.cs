using WardrobeManager.Api.Database.Models;

namespace WardrobeManager.Api.Database;

public class DatabaseInitializer
{
    public static void Initialize(DatabaseContext context)
    {
        if (context.ClothingItems.Count() > 0) { return; }


        var clothingItems = new List<ClothingItem>
        {
            // Creating two base clothing items
            new ClothingItem("Test Clothing 1", Enums.ClothingCategory.TShirt, null),
            new ClothingItem("Test Clothing 2", Enums.ClothingCategory.Sweatpants, null)
        };


        clothingItems.ForEach(d => context.ClothingItems.Add(d));
        context.SaveChanges();
    }
}
