using FooDash.Application.Common.Dtos;

namespace FooDash.Application.Products.Dtos.Basic
{
    public class ReadProductAttributeOptionSetValueDto : ReadDtoBase
    {
        public Guid ProductAttributeId { get; set; }
    }
}