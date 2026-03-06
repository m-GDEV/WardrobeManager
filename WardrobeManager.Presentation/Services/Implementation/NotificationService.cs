#region

using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;

#endregion

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

    public void AddNotification(string title, NotificationType type = NotificationType.Info, string message = "")
    {
        _addNotification(title, type, message);
    }
    
    private void _addNotification(string title, NotificationType type, string message = "")
    {
        lock (_lock)
        {
            var notification = new NotificationMessage(title, type, message);
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
