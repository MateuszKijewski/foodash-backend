using FooDash.Domain.Common.Entities;

namespace FooDash.Domain.Entities.Products
{
    public class Menu : EntityBase
    {
        public string? Description { get; set; }
        public IEnumerable<MenuCategory> MenuCategories { get; set; }
        public bool IsActive { get; set; }
    }
}