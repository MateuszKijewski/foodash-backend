using FooDash.Domain.Common.Entities;

namespace FooDash.Domain.Entities.Prices
{
    public class Currency : EntityBase
    {
        public string Code { get; set; }
        public string Symbol { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool IsBase { get; set; }
    }
}