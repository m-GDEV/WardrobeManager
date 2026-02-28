#region

using System.Security.Cryptography;
using System.Text.RegularExpressions;
using WardrobeManager.Shared.DTOs;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;

#endregion

namespace WardrobeManager.Shared.StaticResources;

public class MiscMethods : IMiscMethods
{
    public string GenerateRandomId()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string id = RandomNumberGenerator.GetString(chars, 8);
        return $"id{id}"; // id needs to start with letter
    }

    public string GetEmoji(ClothingCategory category)
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

    public string GetEmoji(Season season)
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

    public string GetNameWithSpacesFromEnum<T>(T givenEnum) where T : Enum
    {
        return givenEnum switch
        {
            ClothingCategory category => GetNameWithSpacesAndEmoji(category),
            Season season => GetNameWithSpacesAndEmoji(season),
            _ => GetNameWithSpaces(givenEnum)
        };
    }

    private string GetNameWithSpaces<T>(T givenEnum) where T : Enum
    {
        var words = Regex.Matches(givenEnum.ToString(), @"([A-Z][a-z]+)").Select(m => m.Value);
        var withSpaces = string.Join(" ", words);
        return $"{withSpaces}";
    }

    public bool IsValidBase64(string? input)
    {
        try
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            Convert.FromBase64String(input);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public ICollection<T> ConvertEnumToCollection<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T)).Cast<T>().ToList();
    }

    #region Private Methods

    public string GetNameWithSpacesAndEmoji(ClothingCategory category)
    {
        var words = Regex.Matches(category.ToString(), @"([A-Z][a-z]+)").Select(m => m.Value);
        var cat = category;
        var withSpaces = string.Join(" ", words);
        var emoji = GetEmoji(cat);

        return $"{emoji} {withSpaces}";
    }

    public string GetNameWithSpacesAndEmoji(Season season)
    {
        var words = Regex.Matches(season.ToString(), @"([A-Z][a-z]+)").Select(m => m.Value);
        var cat = season;
        var withSpaces = string.Join(" ", words);
        var emoji = GetEmoji(cat);

        return $"{emoji} {withSpaces}";
    }

    #endregion
}