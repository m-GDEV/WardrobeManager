using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Shared.StaticResources;

public interface IMiscMethods
{
    string GenerateRandomId();
    string GetEmoji(ClothingCategory category);
    string GetEmoji(Season season);
    string GetNameWithSpacesAndEmoji(ClothingCategory category);
    string GetNameWithSpacesAndEmoji(Season season);
    string GetNameWithSpacesFromEnum<T>(T givenEnum) where T : Enum;
    bool IsValidBase64(string? input);
    ICollection<T> ConvertEnumToCollection<T>() where T : Enum;
}