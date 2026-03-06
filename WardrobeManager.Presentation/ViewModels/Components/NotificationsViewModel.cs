using Blazing.Mvvm.ComponentModel;
using Blazing.Mvvm.Components;
using Sysinfocus.AspNetCore.Components;
using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Presentation.ViewModels.Components;

[ViewModelDefinition(Lifetime = ServiceLifetime.Scoped)]
public partial class NotificationsViewModel(
    INotificationService notificationService
)
    : ViewModelBase
{
    private Timer? _timer; 
    
    public override async Task OnInitializedAsync()
    {
        notificationService.OnChange += NotifyStateChanged;
        // this runs the callback method supplied every 5 seconds. it does not wait for the last 
        // call to the method to finish. this can cause race conditions. it should be fine though
        _timer = new Timer(TryToClearNotifications, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
    }

    // Go through all notifications and try to remove the non-critical ones
    public void TryToClearNotifications(object? state)
    {
        var notifications = notificationService.Notifications;

        foreach (var notification in notifications)
        {
            TimeSpan difference = DateTime.UtcNow - notification.CreationDate;

            if (difference.TotalSeconds > 10 && notification.Type != NotificationType.Warning && notification.Type != NotificationType.Error)
            {
                notificationService.RemoveNotification(notification);
            }
        }
    }

    public void ClearAllNotifications()
    {
        foreach (var notification in notificationService.Notifications)
        {
            notificationService.RemoveNotification(notification);
        }
    }

    public void Dismiss(NotificationMessage notification)
    {
        notificationService.RemoveNotification(notification);
    }

    public new void Dispose()
    {
        notificationService.OnChange -= NotifyStateChanged;
        _timer?.Dispose();
    }

    public ButtonType GetNotificationButtonType(NotificationType type)
    {
        return type switch
        {
            NotificationType.Error => ButtonType.Destructive,
            _ => ButtonType.Outline
        };
    }

    public string GetNotificationCss(NotificationType type)
    {
        return type switch
        {
            NotificationType.Success => "text-[#28a745]",
            NotificationType.Warning => "text-[#ffc107]",
            NotificationType.Error => "text-[#dc3545]",
            _ => "text-[#007bff]", // default is 'info' colour
        };
    }

    public string GetTruncatedMessage(NotificationMessage notification)
    {
        if (notification.Message.Length >= 40)
        {
            return notification.Message[..37] + "...";
        }

        return notification.Message;
    }
    
}