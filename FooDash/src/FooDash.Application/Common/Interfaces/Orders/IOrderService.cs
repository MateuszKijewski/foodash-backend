using FooDash.Application.Orders.Dtos.Basic;
using FooDash.Application.Orders.Dtos.Contracts;
using FooDash.Domain.Entities.Orders;

namespace FooDash.Application.Common.Interfaces.Orders
{
    public interface IOrderService
    {
        Task<Guid> CreateOrderFromCart(CreateOrderContract createOrderContract);

        Task<IEnumerable<ReadOrderDto>> GetAllOrders();

        Task AssignOrderToCourier(Guid orderId, Guid courierId);

        Task<ReadOrderDto> GetOrder(Guid orderId);

        Task ChangeOrdersStatus(Guid orderId, OrderStatus orderStatus);

        Task CancelOrder(Guid orderId);

        Task<IEnumerable<ReadOrderDto>> GetCourierOrders(Guid courierId);
    }
}