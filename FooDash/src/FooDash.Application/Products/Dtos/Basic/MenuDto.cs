using FooDash.Application.Common.Interfaces.Dtos;

namespace FooDash.Application.Products.Dtos.Basic
{
    public class MenuDto : IDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}