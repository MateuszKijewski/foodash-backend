using FooDash.Domain.Entities.Orders;
using FooDash.Domain.Entities.Prices;
using FooDash.Domain.Entities.Products;

namespace FooDash.Domain.Services.Prices
{
    public class CurrencyExchangeService : ICurrencyExchangeService
    {
        public void CalculatePricesAfterExchange(Product product, Currency targetCurrency)
        {
            product.Price = CalculateAmountAfterExchange(product.Price, targetCurrency);
        }

        public void CalculatePricesAfterExchange(CartItem cartItem, Currency targetCurrency)
        {
            cartItem.Price = CalculateAmountAfterExchange(cartItem.Price, targetCurrency);
        }

        public decimal GetPriceAfterExchange(decimal price, Currency targetCurrency)
        {
            return CalculateAmountAfterExchange(price, targetCurrency);
        }

        private decimal CalculateAmountAfterExchange(decimal amountInBaseCurrency, Currency newCurrency)
        {
            return amountInBaseCurrency * newCurrency.ExchangeRate;
        }
    }
}