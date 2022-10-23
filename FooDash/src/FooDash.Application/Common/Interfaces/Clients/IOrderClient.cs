using FooDash.Application.Orders.Dtos.Basic;

namespace FooDash.Application.Common.Interfaces.Clients
{
    public interface IOrderClient
    {
        Task ReceiveOrderUpdate(ReadOrderDto readOrderDto);
    }
}