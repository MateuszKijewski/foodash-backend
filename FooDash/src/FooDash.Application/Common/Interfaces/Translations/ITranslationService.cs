using FooDash.Application.Common.Interfaces.Dtos;

namespace FooDash.Application.Common.Interfaces.Translations
{
    public interface ITranslationService
    {
        Task<TReadDto> Translate<TReadDto>(TReadDto readDto, Guid languageId)
            where TReadDto : IReadDto;

        Task<IList<TReadDto>> Translate<TReadDto>(IList<TReadDto> readDtos, Guid languageId)
            where TReadDto : IReadDto;
    }
}