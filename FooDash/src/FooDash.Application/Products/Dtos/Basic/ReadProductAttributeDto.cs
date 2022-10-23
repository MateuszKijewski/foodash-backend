using FooDash.Application.Common.Dtos;

namespace FooDash.Application.Products.Dtos.Basic
{
    public class ReadProductAttributeDto : ReadDtoBase
    {
        public Guid ProductId { get; set; }
        public int Order { get; set; }
        public string? TextAttributeValue { get; set; }
        public string? SuffixAttributeValue { get; set; }
        public string? SuffixAttributeSuffix { get; set; }
        public bool? BooleanAttributeValue { get; set; }
    }
}