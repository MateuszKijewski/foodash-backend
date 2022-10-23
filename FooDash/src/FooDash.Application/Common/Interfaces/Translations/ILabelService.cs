using FooDash.Application.Translations.Dtos.Contracts;

namespace FooDash.Application.Common.Interfaces.Translations
{
    public interface ILabelService
    {
        Task SetLabels(SetLabelsContract setLabelsContract);
    }
}