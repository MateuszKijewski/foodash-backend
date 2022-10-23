using FooDash.EssentialDataSeeder.Common;
using FooDash.EssentialDataSeeder.Procedures;

namespace FooDash.EssentialDataSeeder.Helpers
{
    public static class ExtensionMethods
    {
        public static SeedingProcedure AddCommand(this SeedingProcedure installationProcedure, Type commandType)
        {
            var command = (SeedCommand?)Activator.CreateInstance(commandType,
                installationProcedure.Configuration,
                installationProcedure.Services);

            if (command == null)
                throw new ArgumentNullException(nameof(commandType));

            installationProcedure.Commands.Add(command);
            return installationProcedure;
        }
    }
}