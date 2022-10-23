using FooDash.Application.Common.Dtos;

namespace FooDash.Application.Products.Dtos.Basic
{
    public class ReadProductProductAttributeDto : ReadDtoBase
    {
        public Guid ProductId { get; set; }
        public Guid ProductAttributeId { get; set; }
    }
}