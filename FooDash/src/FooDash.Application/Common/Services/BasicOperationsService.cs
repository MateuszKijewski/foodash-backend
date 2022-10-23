using Dawn;
using FooDash.Application.Common.Interfaces.Auth;
using FooDash.Application.Common.Interfaces.Common;
using FooDash.Application.Common.Interfaces.Converters;
using FooDash.Application.Common.Interfaces.Dtos;
using FooDash.Application.Common.Interfaces.Providers;
using FooDash.Application.Common.Interfaces.Repositories;
using FooDash.Application.Common.Interfaces.Translations;
using FooDash.Domain.Common.Entities;

namespace FooDash.Application.Common.Services
{
    public class BasicOperationsService<TEntity, TReadDto, TDto> : IBasicOperationsService<TEntity, TReadDto, TDto>
        where TEntity : EntityBase, new()
        where TReadDto : IReadDto
        where TDto : IDto

    {
        private readonly IBaseRepositoryProvider _baseRepositoryProvider;
        private readonly IEntityConverter _entityConverter;
        private readonly IBaseRepository<TEntity> _genericEntityRepository;
        private readonly ITranslationService _translationService;

        private bool _isEntityTranslated;
        private Guid? _currentUserLanguageId;

        public BasicOperationsService(IBaseRepositoryProvider baseRepositoryProvider,
            IEntityConverter entityConverter,
            IEntityRepository entityRepository,
            ITranslationService translationService,
            ICurrentUserService currentUserService)
        {
            _baseRepositoryProvider = Guard.Argument(baseRepositoryProvider, nameof(baseRepositoryProvider)).NotNull().Value;
            _entityConverter = Guard.Argument(entityConverter, nameof(entityConverter)).NotNull().Value;
            _genericEntityRepository = _baseRepositoryProvider.GetRepository<TEntity>();
            _translationService = Guard.Argument(translationService, nameof(translationService)).NotNull().Value;

            _isEntityTranslated = entityRepository.GetByName(typeof(TEntity).Name).IsTranslated;
            _currentUserLanguageId = currentUserService.User?.LanguageId;
        }

        public async Task<TReadDto> Add(TDto dto)
        {
            var convertedDto = _entityConverter.GetModelFromDto<TEntity, TDto>(dto);
            var addedEntity = await _genericEntityRepository.AddAsync(convertedDto);

            var readDto = _entityConverter.GetReadDtoFromModel<TEntity, TReadDto>(addedEntity);

            if (_currentUserLanguageId.HasValue && _isEntityTranslated)
                return await _translationService.Translate(readDto, _currentUserLanguageId.Value);
            else
                return readDto;
        }

        public async Task<IEnumerable<TReadDto>> AddRange(IEnumerable<TDto> dtos)
        {
            var convertedDtos = dtos.Select(x => _entityConverter.GetModelFromDto<TEntity, TDto>(x));
            var addedEntities = await _genericEntityRepository.AddRangeAsync(convertedDtos);

            var readDtos = addedEntities.Select(x => _entityConverter.GetReadDtoFromModel<TEntity, TReadDto>(x)).ToList();

            if (_currentUserLanguageId.HasValue && _isEntityTranslated)
                return await _translationService.Translate(readDtos, _currentUserLanguageId.Value);
            else
                return readDtos;
        }

        public async Task<IEnumerable<TReadDto>> Get(params Guid[] ids)
        {
            var entities = await _genericEntityRepository.GetAsync(ids);

            var readDtos = entities.Select(x => _entityConverter.GetReadDtoFromModel<TEntity, TReadDto>(x)).ToList();

            if (_currentUserLanguageId.HasValue && _isEntityTranslated)
                return await _translationService.Translate(readDtos, _currentUserLanguageId.Value);
            else
                return readDtos;
        }

        public async Task<IEnumerable<TReadDto>> GetAll()
        {
            var entities = await _genericEntityRepository.GetAllAsync();

            var readDtos = entities.Select(x => _entityConverter.GetReadDtoFromModel<TEntity, TReadDto>(x)).ToList();

            if (_currentUserLanguageId.HasValue && _isEntityTranslated)
                return await _translationService.Translate(readDtos, _currentUserLanguageId.Value);
            else
                return readDtos;
        }

        public async Task Remove(params Guid[] ids)
        {
            await _genericEntityRepository.RemoveRangeAsync(ids);
        }

        public async Task<TReadDto> Update(TDto dto, Guid id)
        {
            var convertedDto = _entityConverter.GetModelFromDto<TEntity, TDto>(dto);
            convertedDto.Id = id;
            var updatedEntity = await _genericEntityRepository.UpdateAsync(convertedDto);

            var readDto = _entityConverter.GetReadDtoFromModel<TEntity, TReadDto>(updatedEntity);

            if (_currentUserLanguageId.HasValue && _isEntityTranslated)
                return await _translationService.Translate(readDto, _currentUserLanguageId.Value);
            else
                return readDto;
        }

        public async Task<IEnumerable<TReadDto>> UpdateRange(IEnumerable<TDto> dtos)
        {
            var convertedDtos = dtos.Select(x => _entityConverter.GetModelFromDto<TEntity, TDto>(x));
            var updatedEntities = await _genericEntityRepository.UpdateRangeAsync(convertedDtos);

            var readDtos = updatedEntities.Select(x => _entityConverter.GetReadDtoFromModel<TEntity, TReadDto>(x)).ToList();

            if (_currentUserLanguageId.HasValue && _isEntityTranslated)
                return await _translationService.Translate(readDtos, _currentUserLanguageId.Value);
            else
                return readDtos;
        }
    }
}