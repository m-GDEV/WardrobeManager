using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Shared.Models;
using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Presentation.Services.Implementation;

public class NotificationService : INotificationService
{
    private readonly List<NotificationMessage> _notifications = new List<NotificationMessage>();
    private readonly object _lock = new object();

    public event Action? OnChange;

    public List<NotificationMessage> Notifications
    {
        get
        {
            lock (_lock)
            {
                return _notifications.ToList();
            }
        }
    }

    public void AddNotification(string message)
    {
        AddNotification(message, NotificationType.Info);
    }
    public void AddNotification(string message, NotificationType type)
    {
        lock (_lock)
        {
            var notification = new NotificationMessage(message, type);
            _notifications.Add(notification);
        }
        NotifyStateChanged();
    }

    public void RemoveNotification(NotificationMessage message)
    {
        lock (_lock)
        {
            _notifications.Remove(message);
        }
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
