using FooDash.Domain.Common.Entities;

namespace FooDash.Domain.Entities.Products
{
    public class ProductAttributeOptionSetValue : EntityBase
    {
        public Guid ProductAttributeId { get; set; }
        public ProductAttribute ProductAttribute { get; set; }
    }
}