using FooDash.Application.Common.Dtos;

namespace FooDash.Application.Prices.Dtos.Basic
{
    public class ReadCurrencyDto : ReadDtoBase
    {
        public string Code { get; set; }
        public string Symbol { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool IsBase { get; set; }
    }
}
