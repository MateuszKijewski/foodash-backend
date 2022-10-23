namespace FooDash.Application.Common.Interfaces.Dtos
{
    public interface IReadDto
    {
        Guid Id { get; }
        string? Name { get; set; }
        string? DisplayName { get; set; }
    }
}