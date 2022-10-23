using FooDash.Application.Common.Dtos;

namespace FooDash.Application.Products.Dtos.Basic
{
    public class ReadMenuCategoryDto : ReadDtoBase
    {
        public int? Order { get; set; }
        public Guid MenuId { get; set; }
        public IEnumerable<ReadProductDto> Products { get; set; }
    }
}