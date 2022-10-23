using FooDash.Application.Metadata.Dtos.Contracts;
using FooDash.Application.Metadata.Dtos.Responses;

namespace FooDash.Application.Common.Interfaces.Metadata
{
    public interface IEntityService
    {
        Task SetEntityTranslatability(SetEntityTranslatabilityContract setEntityTranslatabilityContract);

        IEnumerable<GetEntitiesResponse> GetEntities(RequestedEntitiesType requestedEntitiesType);
    }

    public enum RequestedEntitiesType
    {
        All,
        SystemOnly,
        NonSystemOnly
    }
}