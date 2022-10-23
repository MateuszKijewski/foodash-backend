namespace FooDash.Application.Orders.Dtos.Contracts
{
    public class AssignOrderToCourierContract
    {
        public Guid OrderId { get; set; }
        public Guid CourierId { get; set; }
    }
}