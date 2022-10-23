using FooDash.Application.Common.Interfaces.Dtos;

namespace FooDash.Application.Products.Dtos.Basic
{
    public class MenuCategoryProductDto : IDto
    {
        public int? Order { get; set; }
        public Guid MenuCategoryId { get; set; }
        public Guid ProductId { get; set; }
    }
}