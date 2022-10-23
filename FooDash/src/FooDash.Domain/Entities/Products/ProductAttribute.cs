using FooDash.Domain.Common.Entities;

namespace FooDash.Domain.Entities.Products
{
    public class ProductAttribute : EntityBase
    {
        public int Order { get; set; }
        public AttributeType AttributeType { get; set; }
        public string? TextAttributeValue { get; set; }
        public string? SuffixAttributeValue { get; set; }
        public string? SuffixAttributeSuffix { get; set; }
        public bool? BooleanAttributeValue { get; set; }

        public IEnumerable<ProductProductAttribute> AttributeProducts { get; set; }
        public IEnumerable<ProductAttributeOptionSetValue> ProductAttributeOptionSetValues { get; set; }
    }

    public enum AttributeType
    {
        Text,
        Suffix,
        Boolean,
        OptionSet
    }
}