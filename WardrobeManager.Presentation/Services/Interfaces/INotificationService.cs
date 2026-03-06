#region

using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;

#endregion

namespace WardrobeManager.Presentation.Services.Interfaces;

public interface INotificationService
{
    List<NotificationMessage> Notifications { get; }

    event Action OnChange;

    void AddNotification(string title, NotificationType type = NotificationType.Info, string message = "");
    void RemoveNotification(NotificationMessage message);
}
