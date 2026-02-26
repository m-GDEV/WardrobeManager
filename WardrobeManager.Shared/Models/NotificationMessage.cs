#region

using WardrobeManager.Shared.Enums;

#endregion

namespace WardrobeManager.Shared.Models;

public class NotificationMessage(string message, NotificationType type) {
    public readonly string Message = message;
    public readonly NotificationType Type = type;
    public DateTime CreationDate = DateTime.UtcNow;
}