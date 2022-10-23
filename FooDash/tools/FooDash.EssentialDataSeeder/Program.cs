using FooDash.Application;
using FooDash.EssentialDataSeeder.Commands;
using FooDash.EssentialDataSeeder.Helpers;
using FooDash.EssentialDataSeeder.Procedures;
using FooDash.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

try
{
    var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .AddJsonFile("appsettings.json")
                .Build();

    var services = new ServiceCollection()
        .AddPersistence(configuration)
        .AddApplication(configuration);

    using (var serviceProvider = services.BuildServiceProvider())
    {
        new SeedingProcedure(serviceProvider, configuration)
            .AddCommand(typeof(SeedEntities))
            .AddCommand(typeof(SeedLanguages))
            .AddCommand(typeof(SeedCurrencies))
            .AddCommand(typeof(SeedPermissions))
            .AddCommand(typeof(SeedRoles))
            .AddCommand(typeof(SeedUsers))
            .AddCommand(typeof(SeedSettings))
            .RunProcedure();
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}