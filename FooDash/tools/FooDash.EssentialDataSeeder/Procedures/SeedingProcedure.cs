using Dawn;
using FooDash.EssentialDataSeeder.Common;
using FooDash.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FooDash.EssentialDataSeeder.Procedures
{
    public class SeedingProcedure
    {
        internal readonly IConfiguration Configuration;
        internal readonly IServiceProvider Services;
        internal readonly FooDashDbContext? DbContext;

        public SeedingProcedure(IServiceProvider services, IConfiguration configuration)
        {
            Configuration = Guard.Argument(configuration, nameof(configuration)).NotNull().Value;
            Services = Guard.Argument(services, nameof(services)).NotNull().Value;
            DbContext = Services.GetService<FooDashDbContext>();
        }

        internal List<SeedCommand> Commands = new List<SeedCommand>();

        public void RunProcedure()
        {
            foreach(var command in Commands)
            {
                command.Execute();
            }
        }
    }
}