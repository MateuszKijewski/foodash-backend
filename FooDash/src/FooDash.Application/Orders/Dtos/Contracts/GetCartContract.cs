namespace FooDash.Application.Orders.Dtos.Contracts
{
    public class GetCartContract
    {
        public Guid? CartId { get; set; }
        public Guid? UserLanguageId { get; set; }
    }
}