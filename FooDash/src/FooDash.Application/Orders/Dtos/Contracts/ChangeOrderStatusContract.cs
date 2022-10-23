using FooDash.Domain.Entities.Orders;

namespace FooDash.Application.Orders.Dtos.Contracts
{
    public class ChangeOrderStatusContract
    {
        public Guid OrderId { get; set; }
        public OrderStatus OrderStatus{ get; set; }
    }
}