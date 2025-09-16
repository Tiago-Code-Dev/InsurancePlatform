namespace Shared.CrossCutting.Notifications;

public class Notification
{
    public string? Field { get; }
    public string Message { get; }

    public Notification(string message)
    {
        Message = message;
    }

    public Notification(string field, string message)
    {
        Field = field;
        Message = message;
    }
}
