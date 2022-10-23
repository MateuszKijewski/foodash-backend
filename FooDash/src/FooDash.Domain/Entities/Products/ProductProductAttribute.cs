using FooDash.Domain.Common.Attributes;
using FooDash.Domain.Common.Entities;

namespace FooDash.Domain.Entities.Products
{
    [SystemEntity]
    public class ProductProductAttribute : EntityBase
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Guid ProductAttributeId { get; set; }
        public ProductAttribute ProductAttribute { get; set; }
    }
}