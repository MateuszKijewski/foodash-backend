using FooDash.Application.Common.Interfaces.Dtos;
using System.ComponentModel.DataAnnotations;

namespace FooDash.Application.Products.Dtos.Basic
{
    public class ProductDto : IDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Guid? ProductCategoryId { get; set; }
    }
}