using FooDash.Application.Common.Interfaces.Dtos;
using FooDash.Domain.Common.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FooDash.WebApi.Common.Interfaces
{
    public interface IBaseCrudController<TEntity, TReadDto, TDto>
        where TEntity : EntityBase, new()
        where TReadDto : IReadDto
        where TDto : IDto
    {
        Task<ActionResult<IEnumerable<TReadDto>>> AddRange([FromBody] IEnumerable<TDto> dtos);

        Task<ActionResult<TReadDto>> Get([FromRoute] Guid id);

        Task<ActionResult<IEnumerable<TReadDto>>> GetAll();

        Task<ActionResult> Remove([FromRoute] Guid id);

        Task<ActionResult<TReadDto>> Update([FromBody] TDto dto, [FromRoute] Guid id);
    }
}