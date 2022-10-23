using FooDash.Application.Common.Dtos;

namespace FooDash.Application.Products.Dtos.Basic
{
    public class ReadMenuCategoryProductDto : ReadDtoBase
    {
        public int? Order { get; set; }
        public Guid MenuCategoryId { get; set; }
        public Guid ProductId { get; set; }
    }
}
