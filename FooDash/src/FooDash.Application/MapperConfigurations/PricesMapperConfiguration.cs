using FooDash.Application.Prices.Dtos.Basic;
using FooDash.Domain.Entities.Prices;
using Mapster;

namespace FooDash.Application.MapperConfigurations
{
    public static class PricesMapperConfiguration
    {
        public static TypeAdapterConfig AddPricesConfiguration(this TypeAdapterConfig config)
        {
            config
                .NewConfig<CurrencyDto, Currency>();
            config
                .NewConfig<Currency, ReadCurrencyDto>()
                .IgnoreNullValues(true);

            return config;
        }
    }
}