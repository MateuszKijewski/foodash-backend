using Dawn;
using FooDash.Application.Common.Interfaces.Providers;
using FooDash.Application.Common.Interfaces.Translations;
using FooDash.Application.Translations.Dtos.Contracts;
using FooDash.Domain.Entities.Metadata;
using FooDash.Domain.Entities.Translations;

namespace FooDash.Application.Translations.Services
{
    public class LabelService : ILabelService
    {
        private readonly IBaseRepositoryProvider _baseRepositoryProvider;

        public LabelService(IBaseRepositoryProvider baseRepositoryProvider)
        {
            _baseRepositoryProvider = Guard.Argument(baseRepositoryProvider, nameof(baseRepositoryProvider)).NotNull().Value;
        }

        public async Task SetLabels(SetLabelsContract setLabelsContract)
        {
            
            var labelRepository = _baseRepositoryProvider.GetRepository<Label>();

            await ValidateReferences(setLabelsContract.EntityId, setLabelsContract.LanguageId);

            var labelsToAdd = new List<Label>();
            var labelsToUpdate = new List<Label>();

            Label label;
            Label? existingLabel;
            foreach (var valueWithTranslation in setLabelsContract.ValuesWithTranslations)
            {
                existingLabel = (await labelRepository.FindAsync(x => x.LanguageId == setLabelsContract.LanguageId && x.Value == valueWithTranslation.Value)).FirstOrDefault();
                if (existingLabel != null)
                {
                    existingLabel.TranslatedValue = valueWithTranslation.TranslatedValue;
                    labelsToUpdate.Add(existingLabel);
                }
                else
                {
                    label = new Label
                    {
                        Value = valueWithTranslation.Value,
                        TranslatedValue = valueWithTranslation.TranslatedValue,
                        EntityId = setLabelsContract.EntityId,
                        LanguageId = setLabelsContract.LanguageId
                    };
                    labelsToAdd.Add(label);
                }
            }

            await labelRepository.AddRangeAsync(labelsToAdd);
            await labelRepository.UpdateRangeAsync(labelsToUpdate);
        }

        private async Task ValidateReferences(Guid entityId, Guid languageId)
        {
            var entityRepository = _baseRepositoryProvider.GetRepository<Entity>();
            var languageRepository = _baseRepositoryProvider.GetRepository<Language>();

            var entity = await entityRepository.GetAsync(entityId);
            if (!entity.IsTranslated)
                throw new InvalidOperationException($"Labels for entity {entity.Name} cannot be installed, it's not set to be translatable");
            await languageRepository.GetAsync(languageId);
        }
    }
}