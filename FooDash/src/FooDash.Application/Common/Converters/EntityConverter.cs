using Dawn;
using FooDash.Application.Common.Interfaces.Converters;
using MapsterMapper;

namespace FooDash.Application.Common.Converters
{
    public class EntityConverter : IEntityConverter
    {
        private readonly IMapper _mapper;

        public EntityConverter(IMapper mapper)
        {
            _mapper = Guard.Argument(mapper, nameof(mapper)).NotNull().Value;
        }

        public TEntity GetModelFromDto<TEntity, TDto>(TDto baseDto)
        {
            return _mapper.Map<TEntity>(baseDto);
        }

        public TReadDto GetReadDtoFromModel<TEntity, TReadDto>(TEntity model)
        {
            return _mapper.Map<TReadDto>(model);
        }
    }
}