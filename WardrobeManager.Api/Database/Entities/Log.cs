#region

using WardrobeManager.Shared.Enums;

#endregion

namespace WardrobeManager.Api.Database.Entities;
public class Log(string title, string description, LogType type) : IDatabaseEntity
{
    public int PrimaryKeyId { get; set; }
    public string Title { get; set; } = title;
    public string Description { get; set; } = description;
    public LogType Type { get; set; } = type;
    public DateTime Created { get; set; } = DateTime.UtcNow;
}

