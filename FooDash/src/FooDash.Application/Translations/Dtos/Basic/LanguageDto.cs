using FooDash.Application.Common.Interfaces.Dtos;
using System.ComponentModel.DataAnnotations;

namespace FooDash.Application.Translations.Dtos.Basic
{
    public class LanguageDto : IDto
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
    }
}