namespace FooDash.Application.Common.Interfaces.Clients
{    
    public interface INotificationClient
    {
        Task ReceiveTestNotification(TestNotification testNotification);
    }
    public class TestNotification
    {
        public string? Message { get; set; }
    }
}