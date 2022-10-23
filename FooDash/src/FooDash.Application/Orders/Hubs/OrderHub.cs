using FooDash.Application.Common.Interfaces.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.SignalR;

namespace FooDash.Application.Orders.Hubs
{
    [AllowAnonymous]
    public class OrderHub : Hub<IOrderClient>
    {
        public override async Task OnConnectedAsync()
        {
            var httpContext = ((IHttpContextFeature)Context.Features[typeof(IHttpContextFeature)]).HttpContext;
            var orderId = httpContext.Request.Query["orderId"].First();

            await Groups.AddToGroupAsync(Context.ConnectionId, orderId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var httpContext = ((IHttpContextFeature)Context.Features[typeof(IHttpContextFeature)]).HttpContext;
            var orderId = httpContext.Request.Query["orderId"].First();

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, orderId);

            await base.OnDisconnectedAsync(exception);
        }

    }
}