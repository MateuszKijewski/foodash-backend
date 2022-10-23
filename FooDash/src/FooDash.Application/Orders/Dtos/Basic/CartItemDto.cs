using FooDash.Application.Common.Interfaces.Dtos;

namespace FooDash.Application.Orders.Dtos.Basic
{
    public class CartItemDto : IDto
    {
        public decimal Price { get; set; }
        public Guid CurrencyId { get; set; }
        public Guid CartId { get; set; }
    }
}