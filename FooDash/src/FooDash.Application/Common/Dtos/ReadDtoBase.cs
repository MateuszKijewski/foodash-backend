using FooDash.Application.Common.Interfaces.Dtos;

namespace FooDash.Application.Common.Dtos
{
    public abstract class ReadDtoBase : IReadDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatorId { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifierId { get; set; }
    }
}