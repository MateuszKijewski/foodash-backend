using FooDash.Application.Common.Dtos;
using FooDash.Application.Prices.Dtos.Basic;

namespace FooDash.Application.Orders.Dtos.Basic
{
    public class ReadOrderItemDto : ReadDtoBase
    {
        public decimal Price { get; set; }
        public Guid CurrencyId { get; set; }
        public ReadCurrencyDto Currency { get; set; }
        public Guid OrderId { get; set; }
    }
}