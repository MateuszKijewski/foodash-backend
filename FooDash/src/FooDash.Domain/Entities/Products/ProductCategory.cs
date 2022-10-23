using FooDash.Domain.Common.Entities;

namespace FooDash.Domain.Entities.Products
{
    public class ProductCategory : EntityBase
    {
        public string? Description { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}