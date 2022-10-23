using MediatR;

namespace FooDash.Application.Metadata.Dtos.Contracts
{
    public class SetEntityTranslatabilityContract
    {
        public Guid EntityId { get; set; }
        public bool IsTranslatable { get; set; }
    }
}