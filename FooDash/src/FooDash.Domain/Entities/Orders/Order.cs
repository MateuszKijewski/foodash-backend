using FooDash.Domain.Common.Entities;
using FooDash.Domain.Entities.Identity;
using FooDash.Domain.Entities.Prices;

namespace FooDash.Domain.Entities.Orders
{
    public class Order : EntityBase
    {
        public decimal Price { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string? Notes { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public Guid CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public Guid? OrderingUserId { get; set; }
        public User OrderingUser { get; set; }
        public Guid? CourierId { get; set; }
        public User Courier { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal DeliveryCost { get; set; }
        public OrderStatus Status { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set;}
    }

    public enum OrderStatus
    {
        New,
        Paid,
        Pickup,
        InProgress,
        Delivered,
        Canceled
    }
}