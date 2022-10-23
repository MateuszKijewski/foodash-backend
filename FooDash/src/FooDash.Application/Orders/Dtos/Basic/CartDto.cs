using FooDash.Application.Common.Interfaces.Dtos;

namespace FooDash.Application.Orders.Dtos.Basic
{
    public class CartDto : IDto
    {
        public Guid CurrencyId { get; set; }
        public Guid? UserId { get; set; }
    }
}