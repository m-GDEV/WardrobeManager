using System.Text.RegularExpressions;
using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Shared.Misc;

public static class ProjectConstants
{
    public static readonly string ProjectName = "Wardrobe Manager";
    // Version is shared between Presentation and API as they are developed at the same time, and 
    // they will always work together so long as they have the same version
    public const string ProjectVersion = "0.1.0"; // uses SemVer
    public static readonly string ProjectGitRepo = $"https://github.com/m-GDEV/{ProjectName.Replace(" ", "")}";
    public static readonly string ProfileImage = "https://upload.internal.connectwithmusa.com/file/eel-falcon-pig";
    public static readonly string DefaultItemImage = "/img/defaultItem.webp";
    public static readonly string HomeBackgroundImage = "/img/home-background.webp";
}
