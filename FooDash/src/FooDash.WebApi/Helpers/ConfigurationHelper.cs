using FooDash.Application.Notifications.Hubs;
using FooDash.Application.Orders.Hubs;

namespace FooDash.WebApi.Helpers
{
    public static class ConfigurationHelper
    {
        public static string[] GetAllowedOrigins(IConfiguration configuration)
        {
            string hosts = configuration.GetValue<string>("AllowedOrigins");
            return hosts.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static void ConfigureSignalR(this WebApplication webApplication)
        {
            webApplication.UseEndpoints(routes => routes.MapHub<NotificationHub>("/ws/notifications"));
            webApplication.UseEndpoints(routes => routes.MapHub<OrderHub>("/ws/orders"));
        }
    }
}
