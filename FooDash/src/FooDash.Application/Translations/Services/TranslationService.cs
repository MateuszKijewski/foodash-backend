using FooDash.Application.Common.Interfaces.Providers;
using FooDash.Application.Common.Interfaces.Translations;
using FooDash.Application.Common.Interfaces.Dtos;
using FooDash.Domain.Entities.Translations;

namespace FooDash.Application.Translations.Services
{
    public class TranslationService : ITranslationService
    {
        private readonly IBaseRepositoryProvider _baseRepositoryProvider;

        public TranslationService(IBaseRepositoryProvider baseRepositoryProvider)
        {
            _baseRepositoryProvider = baseRepositoryProvider;
        }

        public async Task<TReadDto> Translate<TReadDto>(TReadDto readDto, Guid languageId)
            where TReadDto : IReadDto
        {
            return (await PerformTranslation(new List<TReadDto> { readDto }, languageId)).First();
        }

        public async Task<IList<TReadDto>> Translate<TReadDto>(IList<TReadDto> readDtos, Guid languageId)
            where TReadDto : IReadDto
        {
            return await PerformTranslation(readDtos, languageId);
        }

        private async Task<IList<TReadDto>> PerformTranslation<TReadDto>(IList<TReadDto> readDtos, Guid languageId)
            where TReadDto : IReadDto
        {
            var labelRepository = _baseRepositoryProvider.GetRepository<Label>();

            foreach (var readDto in readDtos)
            {
                readDto.DisplayName = (await labelRepository.FindAsync(x => x.LanguageId == languageId && x.Value == readDto.Name)).SingleOrDefault()?.TranslatedValue ?? readDto.Name;
            }

            return readDtos;
        }
    }
}
