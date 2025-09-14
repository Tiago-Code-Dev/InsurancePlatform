namespace Shared.CrossCutting.Notifications;

public class Notifier : INotifier
{
    private readonly List<Notification> _notifications = new();

    public void Handle(Notification notification)
    {
        _notifications.Add(notification);
    }

    public List<Notification> GetNotifications()
    {
        return _notifications;
    }

    public bool HasNotification()
    {
        return _notifications.Any();
    }
}
