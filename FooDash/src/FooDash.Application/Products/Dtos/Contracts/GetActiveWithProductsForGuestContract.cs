namespace FooDash.Application.Products.Dtos.Contracts
{
    public class GetActiveWithProductsForGuestContract
    {
        public Guid GuestCurrencyId { get; set; }
        public Guid GuestLanguageId { get; set; }
    }
}