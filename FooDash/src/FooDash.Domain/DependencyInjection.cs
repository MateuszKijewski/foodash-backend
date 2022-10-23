using FooDash.Domain.Services.Prices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FooDash.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ICurrencyExchangeService, CurrencyExchangeService>();

            return services;
        }
    }
}