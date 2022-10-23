using MediatR;

namespace FooDash.Application.Orders.Dtos.Contracts
{
    public class SendTestOrderContract : IRequest
    {
        public string OrderNumber { get; set; }
    }
}