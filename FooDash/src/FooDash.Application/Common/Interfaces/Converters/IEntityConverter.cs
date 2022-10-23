namespace FooDash.Application.Common.Interfaces.Converters
{
    public interface IEntityConverter
    {
        TReadDto GetReadDtoFromModel<TEntity, TReadDto>(TEntity model);

        TEntity GetModelFromDto<TEntity, TDto>(TDto baseDto);
    }
}