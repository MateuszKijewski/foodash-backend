using FooDash.Domain.Common.Attributes;
using FooDash.Domain.Common.Entities;

namespace FooDash.Domain.Entities.Products
{
    [SystemEntity]
    public class MenuCategoryProduct : EntityBase
    {
        public int? Order { get; set; }
        public Guid MenuCategoryId { get; set; }
        public MenuCategory MenuCategory { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}