using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Shared.Models;

public class NotificationMessage(string Message, NotificationType Type) {
    public string message = Message;
    public NotificationType type = Type;
    public DateTime CreationDate = DateTime.UtcNow;
}