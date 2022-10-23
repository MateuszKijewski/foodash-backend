using FooDash.Application.Common.Interfaces.Dtos;

namespace FooDash.Application.Prices.Dtos.Basic
{
    public class CurrencyDto : IDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Symbol { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool IsBase { get; set; }
    }
}