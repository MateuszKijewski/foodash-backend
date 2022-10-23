using FooDash.Application.Common.Dtos;

namespace FooDash.Application.Products.Dtos.Basic
{
    public class ReadProductDto : ReadDtoBase
    {
        public decimal Price { get; set; }
        public Guid? ProductCategoryId { get; set; }
    }
}