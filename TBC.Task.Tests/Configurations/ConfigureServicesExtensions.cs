using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TBC.Task.API.Controllers;
using TBC.Task.Domain.Interfaces.Repositories;
using TBC.Task.Domain.Interfaces.Services;
using TBC.Task.Repository;
using TBC.Task.Repository.Database;
using TBC.Task.Service;

namespace TBC.Task.Tests.Configurations;

public static class ConfigureServicesExtensions
{
    public static IServiceCollection ConfigureServices(
        this IServiceCollection services,
        IHostEnvironment environment,
        IConfiguration configuration)
    {
        services.AddSingleton(environment);
        services.AddSingleton(configuration);
        services.AddLogging();
        services.AddLocalization();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddTransient<ICityRepository, CityRepository>();
        services.AddTransient<IPersonRepository, PersonRepository>();
        services.AddTransient<IRelatedPersonRepository, RelatedPersonRepository>();

        services.AddTransient<ICityService, CityService>();
        services.AddTransient<IPersonService, PersonService>();
        services.AddTransient<IRelatedPersonService, RelatedPersonService>();

        services.AddTransient<PersonsController>();

        return services;
    }

    public static IServiceCollection ConfigureDbContext(this IServiceCollection services)
    {
        services.AddDbContext<PersonsDbContext>(options =>
            options.UseInMemoryDatabase("TbcPersonsInMemoryDB"));

        return services;
    }
}
