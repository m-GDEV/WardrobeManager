using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Shared.Misc;

public static class ProjectConstants {
    public static string Name = "Wardrobe Manager";
    public static string ProfileImage = "https://upload.vps.connectwithmusa.com/file/sloth-crow-zebra";
    public static string DefaultItemImage = "https://upload.vps.connectwithmusa.com/file/spider-pig-squid";

    public static string GetEmojiForClothingCategory(ClothingCategory category) {
        return category switch {
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
    public static string GetEmojiForSeason(Season season) {
        return season switch {
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
}
