using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FooDash.WebApi.Helpers
{
    public static class ControllerHelper
    {
        public static string GetErrorsFromModelState(ModelStateDictionary modelStateDictionary)
        {
            var errors = modelStateDictionary.Select(x => x.Value.Errors)
                          .Where(y => y.Count > 0)
                          .Select(x => x.Select(y => y.ErrorMessage))
                          .SelectMany(x => x)
                          .ToList();

            return string.Join(", ", errors);
        }
    }
}
