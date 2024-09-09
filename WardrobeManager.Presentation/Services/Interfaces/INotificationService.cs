using WardrobeManager.Presentation.Services.Implementation;

namespace WardrobeManager.Presentation.Services.Interfaces;

public interface INotificationService
{
    List<NotificationMessage> Notifications { get; }

    event Action OnChange;

    void AddNotification(string message, NotificationType type);
    void RemoveNotification(NotificationMessage message);
}
