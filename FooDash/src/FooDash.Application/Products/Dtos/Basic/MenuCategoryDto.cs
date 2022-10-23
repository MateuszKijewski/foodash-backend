using FooDash.Application.Common.Interfaces.Dtos;

namespace FooDash.Application.Products.Dtos.Basic
{
    public class MenuCategoryDto : IDto
    {
        public string Name { get; set; }
        public int? Order { get; set; }
        public Guid MenuId { get; set; }
    }
}