namespace FooDash.Application.Orders.Dtos.Contracts
{
    public class AddProductToCartContract
    {
        public Guid ProductId { get; set; }
        public Guid? UserCurrencyId { get; set; }
        public Guid? CartId { get; set; }
    }
}