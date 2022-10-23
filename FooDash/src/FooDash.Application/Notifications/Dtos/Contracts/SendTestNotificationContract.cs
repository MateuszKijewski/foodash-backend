using MediatR;
namespace FooDash.Application.Notifications.Dtos.Contracts
{
    public class SendTestNotificationContract
    {
        public string Message { get; set; }
    }
}