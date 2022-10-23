using FooDash.Application.Common.Interfaces.Dtos;
using System.ComponentModel.DataAnnotations;

namespace FooDash.Application.Products.Dtos.Basic
{
    public class ProductCategoryDto : IDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}