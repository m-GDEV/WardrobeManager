#region

using WardrobeManager.Shared.Enums;

#endregion

namespace WardrobeManager.Api.Database.Entities;
public class Log(string title, string description, LogType type, LogOrigin origin) : IDatabaseEntity
{
    public int PrimaryKeyId { get; set; }
    public string Title { get; set; } = title;
    public string Description { get; set; } = description;
    public LogType Type { get; set; } = type;
    public LogOrigin Origin { get; set; } = origin;
    public DateTime Created { get; set; } = DateTime.UtcNow;
}

