using FooDash.Domain.Common.Entities;

namespace FooDash.Domain.Entities.Products
{
    public class MenuCategory : EntityBase
    {
        public Guid MenuId { get; set; }
        public Menu Menu { get; set; }
        public int? Order { get; set; }
        public IEnumerable<MenuCategoryProduct> MenuCategoryProducts { get; set; }
    }
}