using FooDash.Application.Common.Dtos;
using FooDash.Application.Prices.Dtos.Basic;
using FooDash.Application.Users.Dtos.Basic;

namespace FooDash.Application.Orders.Dtos.Basic
{
    public class ReadCartDto : ReadDtoBase
    {
        public decimal Value { get; set; }
        public Guid CurrencyId { get; set; }
        public ReadCurrencyDto Currency { get; set; }
        public Guid? UserId { get; set; }
        public ReadUserDto User { get; set; }
        public IEnumerable<ReadCartItemDto> CartItems { get; set; }
    }
}