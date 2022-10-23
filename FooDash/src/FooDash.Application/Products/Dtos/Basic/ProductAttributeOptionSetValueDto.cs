using FooDash.Application.Common.Interfaces.Dtos;
using System.ComponentModel.DataAnnotations;

namespace FooDash.Application.Products.Dtos.Basic
{
    public class ProductAttributeOptionSetValueDto : IDto
    {
        public string Name { get; set; }
        public Guid ProductAttributeId { get; set; }
    }
}