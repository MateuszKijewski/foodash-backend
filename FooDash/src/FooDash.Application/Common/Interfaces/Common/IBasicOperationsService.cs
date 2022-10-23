using FooDash.Application.Common.Interfaces.Dtos;
using FooDash.Domain.Common.Entities;

namespace FooDash.Application.Common.Interfaces.Common
{
    public interface IBasicOperationsService<TEntity, TReadDto, TDto>
        where TEntity : EntityBase
        where TReadDto : IReadDto
        where TDto : IDto
    {
        Task<IEnumerable<TReadDto>> Get(params Guid[] ids);

        Task<IEnumerable<TReadDto>> GetAll();

        Task<TReadDto> Add(TDto dto);

        Task<IEnumerable<TReadDto>> AddRange(IEnumerable<TDto> dtos);

        Task<TReadDto> Update(TDto dto, Guid id);

        Task<IEnumerable<TReadDto>> UpdateRange(IEnumerable<TDto> dtos);

        Task Remove(params Guid[] ids);
    }
}