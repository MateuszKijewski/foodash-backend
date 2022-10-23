using FooDash.Domain.Entities.Prices;
using FooDash.Domain.Entities.Translations;

namespace FooDash.Application.System.Dtos.Responses
{
    public class GetSettingsResponse
    {
        public Guid DefaultCurrencyId { get; set; }
        public Currency DefaultCurrency { get; set; }
        public Guid DefaultLanguageId { get; set; }
        public Language DefaultLanguage { get; set; }
        public decimal DeliveryPriceInMainCurrency { get; set; }
    }
}