using FooDash.Application.Common.Interfaces.Dtos;
using FooDash.Domain.Entities.Products;

namespace FooDash.Application.Products.Dtos.Basic
{
    public class ProductAttributeDto : IDto
    {
        public string Name { get; set; }
        public int? Order { get; set; }
        public AttributeType AttributeType { get; set; }
        public string? TextAttributeValue { get; set; }
        public string? SuffixAttributeValue { get; set; }
        public string? SuffixAttributeSuffix { get; set; }
        public bool? BooleanAttributeValue { get; set; }
    }
}