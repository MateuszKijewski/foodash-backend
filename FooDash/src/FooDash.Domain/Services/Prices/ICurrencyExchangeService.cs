using FooDash.Domain.Entities.Orders;
using FooDash.Domain.Entities.Prices;
using FooDash.Domain.Entities.Products;

namespace FooDash.Domain.Services.Prices
{
    public interface ICurrencyExchangeService
    {
        void CalculatePricesAfterExchange(Product product, Currency targetCurrency);

        void CalculatePricesAfterExchange(CartItem cartItem, Currency targetCurrency);

        decimal GetPriceAfterExchange(decimal price, Currency targetCurrency);
    }
}