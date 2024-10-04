using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Shared.Misc;

public static class ProjectConstants {
    public static string Name = "Wardrobe Manager";
    public static string ProfileImage = "https://upload.internal.connectwithmusa.com/file/eel-pug-tiger";
    public static string DefaultItemImage = "https://image.hm.com/assets/hm/a3/f5/a3f56f6e47160e931b78296bb9e479bfbcab3554.jpg?imwidth=2160";

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
