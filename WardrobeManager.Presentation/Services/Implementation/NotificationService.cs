using WardrobeManager.Presentation.Services.Interfaces;

namespace WardrobeManager.Presentation.Services.Implementation;

public class NotificationService : INotificationService
{
    private readonly List<string> _notifications = new List<string>();
    private readonly object _lock = new object();

    public event Action OnChange;

    public List<string> Notifications
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
        lock (_lock)
        {
            _notifications.Add(message);
        }
        NotifyStateChanged();
    }

    public void RemoveNotification(string message)
    {
        lock (_lock)
        {
            _notifications.Remove(message);
        }
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
