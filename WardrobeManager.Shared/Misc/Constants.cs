using System.Text.RegularExpressions;

using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Shared.Misc;

public static class ProjectConstants
{
    public static string Name = "Wardrobe Manager";
    public static string ApiUrl = "https://localhost:7026"; // should probably change this, also it should not be in the Shared project
    public static string ProfileImage = "https://upload.internal.connectwithmusa.com/upload/eel-falcon-pig";
    public static string DefaultItemImage = "https://image.hm.com/assets/hm/a3/f5/a3f56f6e47160e931b78296bb9e479bfbcab3554.jpg?imwidth=2160";

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
            Season.SprintAndSummer => "🌸☀️",
            Season.SummerAndFall => "☀️🍂",
            _ => ""
        };
    }

    public static string GetNameWithSpacesAndEmoji(ClothingCategory category)
    {
        var words = Regex.Matches(category.ToString(), @"([A-Z][a-z]+)").Cast<Match>().Select(m => m.Value);
        var cat = (ClothingCategory)category;
        var withSpaces = string.Join(" ", words);
        var emoji = GetEmoji(cat);

        return $"{emoji} {withSpaces}";
    }
    public static string GetNameWithSpacesAndEmoji(Season season)
    {
        var words = Regex.Matches(season.ToString(), @"([A-Z][a-z]+)").Cast<Match>().Select(m => m.Value);
        var cat = (Season)season;
        var withSpaces = string.Join(" ", words);
        var emoji = GetEmoji(cat);

        return $"{emoji} {withSpaces}";
    }
}
