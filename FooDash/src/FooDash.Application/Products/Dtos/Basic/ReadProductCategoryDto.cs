using FooDash.Application.Common.Dtos;

namespace FooDash.Application.Products.Dtos.Basic
{
    public class ReadProductCategoryDto : ReadDtoBase
    {
        public string? Description { get; set; }
    }
}