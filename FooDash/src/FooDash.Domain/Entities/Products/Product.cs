using FooDash.Domain.Common.Entities;

namespace FooDash.Domain.Entities.Products
{
    public class Product : EntityBase
    {
        public decimal Price { get; set; }
        public Guid? ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }

        public IEnumerable<ProductProductAttribute> ProductAttributes { get; set; }
    }
}