#region

using WardrobeManager.Shared.Enums;

#endregion

namespace WardrobeManager.Shared.Models;

public class NotificationMessage(string title, NotificationType type, string message = "") {
    public readonly string Title = title;
    // body of the notification. not always used, but useful for showing exception stack traces
    public readonly string Message = message;
    public readonly NotificationType Type = type;
    public bool DialogOpen = false;
    public readonly DateTime CreationDate = DateTime.UtcNow;
}