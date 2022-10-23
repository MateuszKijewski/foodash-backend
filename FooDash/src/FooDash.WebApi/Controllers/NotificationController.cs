using FooDash.Application.Common.Interfaces.Clients;
using FooDash.Application.Notifications.Dtos.Contracts;
using FooDash.Application.Notifications.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace FooDash.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<NotificationHub, INotificationClient> _notificationHub;

        public NotificationController(IHubContext<NotificationHub, INotificationClient> notificationHub)
        {
            _notificationHub = notificationHub;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> TestNotification([FromBody] SendTestNotificationContract contract)
        {
            var notification = new TestNotification
            {
                Message = contract.Message
            };
            await _notificationHub.Clients.All.ReceiveTestNotification(notification);

            return Ok();
        }
    }
}