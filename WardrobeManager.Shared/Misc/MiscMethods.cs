using System.Text.RegularExpressions;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Shared.Misc;

public static class MiscMethods
{
    
    public static NewOrEditedClothingItemDTO CreateDefaultNewOrEditedClothingItemDTO() {
        return new NewOrEditedClothingItemDTO("My Favourite Green T-Shirt", ClothingCategory.TShirt, Season.Fall, false, WearLocation.HomeAndOutside, 5, "");
    }
    
    // This is by no means actually random
    public static string GenerateRandomId()
    {
        Random random = new Random(); // spikcq: CS-A1008
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var id = new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
        return $"id{id}"; // id needs to start with letter
    }
    public static string GetEmoji(ClothingCategory category)
    {
        return category switch
        {
            ClothingCategory.TShirt => "👕",
            ClothingCategory.DressShirt => "👔",
            ClothingCategory.Sweater => "🧥",
            ClothingCategory.Shorts => "🩳",
            ClothingCategory.Sweatpants => "👖",
            ClothingCategory.Jeans => "👖",
            ClothingCategory.DressPants => "👖",
            _ => ""
        };
    }
    public static string GetEmoji(Season season)
    {
        return season switch
        {
            Season.Fall => "🍂",
            Season.Winter => "❄️",
            Season.Spring => "🌸",
            Season.Summer => "☀️",

            Season.FallAndWinter => "🍂❄️",
            Season.WinterAndSpring => "❄️🌸",
            Season.SpringAndSummer => "🌸☀️",
            Season.SummerAndFall => "☀️🍂",
            _ => ""
        };
    }

    public static string GetNameWithSpacesAndEmoji(ClothingCategory category)
    {
        var words = Regex.Matches(category.ToString(), @"([A-Z][a-z]+)").Select(m => m.Value);
        var cat = category;
        var withSpaces = string.Join(" ", words);
        var emoji = GetEmoji(cat);

        return $"{emoji} {withSpaces}";
    }
    public static string GetNameWithSpacesAndEmoji(Season season)
    {
        var words = Regex.Matches(season.ToString(), @"([A-Z][a-z]+)").Select(m => m.Value);
        var cat = season;
        var withSpaces = string.Join(" ", words);
        var emoji = GetEmoji(cat);

        return $"{emoji} {withSpaces}";
    }
    
    public static string GetNameWithSpacesFromEnum<T>(T givenEnum) where T : Enum {
        var words = Regex.Matches(givenEnum.ToString(), @"([A-Z][a-z]+)").Select(m => m.Value);
        var withSpaces = string.Join(" ", words);
        return $"{withSpaces}";
    }
}