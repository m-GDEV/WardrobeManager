#region

using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;

#endregion

namespace WardrobeManager.Presentation.Services.Interfaces;

public interface INotificationService
{
    List<NotificationMessage> Notifications { get; }

    event Action OnChange;

    void AddNotification(string message);
    void AddNotification(string message, NotificationType type);
    void RemoveNotification(NotificationMessage message);
}
