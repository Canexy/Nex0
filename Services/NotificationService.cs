// Services/NotificationService.cs
using Avalonia.Controls;
using Avalonia.Controls.Notifications;

namespace Nexo.Services
{
    public class NotificationService
    {
        private WindowNotificationManager? _notificationManager;
        
        public void Initialize(Window hostWindow)
        {
            _notificationManager = new WindowNotificationManager(hostWindow)
            {
                Position = NotificationPosition.TopRight,
                MaxItems = 3
            };
        }
        
        public void ShowNotification(string title, string message, NotificationType type = NotificationType.Information)
        {
            _notificationManager?.Show(new Notification(title, message, type));
        }
    }
}