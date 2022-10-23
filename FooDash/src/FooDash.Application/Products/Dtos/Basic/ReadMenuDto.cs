using FooDash.Application.Common.Dtos;

namespace FooDash.Application.Products.Dtos.Basic
{
    public class ReadMenuDto : ReadDtoBase
    {
        public string? Description { get; set; }
        public IEnumerable<ReadMenuCategoryDto> Categories { get; set; }
    }
}