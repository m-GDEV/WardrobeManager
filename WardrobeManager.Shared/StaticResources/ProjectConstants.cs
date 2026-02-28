namespace WardrobeManager.Shared.StaticResources;

public static class ProjectConstants
{
    public const string ProjectName = "Wardrobe Manager";
    // Version is shared between Presentation and API as they are developed at the same time, and 
    // they will always work together so long as they have the same version
    public const string ProjectVersion = "0.1.1"; // uses SemVer
    public static readonly string ProjectGitRepo = $"https://github.com/m-GDEV/{ProjectName.Replace(" ", "")}";
    public const string ProfileImage = "https://upload.internal.connectwithmusa.com/file/eel-falcon-pig";
    public const string DefaultItemImage = "/img/defaultItem.webp";
    public const string HomeBackgroundImage = "/img/home-background.webp";
    public const int MaxImageSizeInMBFallback = 5;
}
