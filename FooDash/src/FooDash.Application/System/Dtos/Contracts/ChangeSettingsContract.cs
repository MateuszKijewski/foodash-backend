namespace FooDash.Application.System.Dtos.Contracts
{
    public class ChangeSettingsContract
    {
        public Guid? DefaultCurrencyId { get; set; }
        public Guid? DefaultLanguageId { get; set; }
        public decimal? DeliveryPriceInMainCurrency { get; set; }
    }
}