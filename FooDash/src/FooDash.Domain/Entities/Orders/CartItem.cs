using FooDash.Domain.Common.Entities;
using FooDash.Domain.Entities.Prices;

namespace FooDash.Domain.Entities.Orders
{
    public class CartItem : EntityBase
    {
        public decimal Price { get; set; }
        public Guid CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public Guid CartId { get; set; }
        public Cart Cart { get; set; }
    }
}