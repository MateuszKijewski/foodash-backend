using FooDash.Application.Common.Interfaces.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FooDash.Application.Notifications.Hubs
{
    [AllowAnonymous]
    public class NotificationHub : Hub<INotificationClient>
    { }
}