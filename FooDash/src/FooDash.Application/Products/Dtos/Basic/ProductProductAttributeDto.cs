using FooDash.Application.Common.Interfaces.Dtos;

namespace FooDash.Application.Products.Dtos.Basic
{
    public class ProductProductAttributeDto : IDto
    {
        public Guid ProductId { get; set; }
        public Guid ProductAttributeId { get; set; }
    }
}
