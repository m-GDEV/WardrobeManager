namespace WardrobeManager.Presentation.Identity.Models;

/// <summary>
/// Response for login and registration.
/// </summary>
public class FormResult
{

    private static string[] DefaultArray = [""];
    
    /// <summary>
    /// Gets or sets a value indicating whether the action was successful.
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    /// On failure, the problem details are parsed and returned in this array.
    /// </summary>
    
    // Fixes stupid DeepSource warning
    public string[] ErrorList { get; set; } = (string[])DefaultArray.Clone();
}