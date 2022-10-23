using FooDash.Application.Common.Dtos;
using FooDash.Application.Prices.Dtos.Basic;
using FooDash.Application.Users.Dtos.Basic;
using FooDash.Domain.Entities.Orders;

namespace FooDash.Application.Orders.Dtos.Basic
{
    public class ReadOrderDto : ReadDtoBase
    {
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
        public ReadCurrencyDto Currency { get; set; }
        public Guid? OrderingUserId { get; set; }
        public ReadUserDto OrderingUser { get; set; }
        public Guid? CourierId { get; set; }
        public ReadUserDto Courier { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal DeliveryCost { get; set; }
        public OrderStatus Status { get; set; }
        public IEnumerable<ReadOrderItemDto> OrderItems { get; set; }
    }
}