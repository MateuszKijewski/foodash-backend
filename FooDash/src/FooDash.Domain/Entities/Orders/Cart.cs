using FooDash.Domain.Common.Entities;
using FooDash.Domain.Entities.Identity;
using FooDash.Domain.Entities.Prices;

namespace FooDash.Domain.Entities.Orders
{
    public class Cart : EntityBase
    {
        public decimal Value { get; set; }
        public Guid CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public Guid? UserId { get; set; }
        public User User { get; set; }
        public IEnumerable<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}