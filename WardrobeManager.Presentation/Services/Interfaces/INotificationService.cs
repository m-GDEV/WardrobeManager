namespace WardrobeManager.Presentation.Services.Interfaces;

public interface INotificationService
{
    List<string> Notifications { get; }

    event Action OnChange;

    void AddNotification(string message);
    void RemoveNotification(string message);
}