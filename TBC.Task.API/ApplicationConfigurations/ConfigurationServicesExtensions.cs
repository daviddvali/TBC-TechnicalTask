using Microsoft.EntityFrameworkCore;
using TBC.Task.API.ActionFilters;
using TBC.Task.API.Serilog;
using TBC.Task.API.Swagger;
using TBC.Task.Repository;
using TBC.Task.Repository.Database;
using TBC.Task.Service;
using TBC.Task.Service.Interfaces.Repositories;
using TBC.Task.Service.Interfaces.Services;

namespace TBC.Task.API.ApplicationConfigurations;

public static class ConfigurationServicesExtensions
{
	public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
	{
		var configuration = builder.Configuration;

		builder.Services.AddControllers(options =>
		{
			options.Filters.Add<PersonRequestDataValidationAttribute>();
			options.Filters.Add<RelatedPersonRequestDataValidationAttribute>();
			options.Filters.Add<SearchRequestDataValidationAttribute>();
		});
		builder.Services.AddEndpointsApiExplorer();

		builder.ConfigureSwagger();
		builder.ConfigureLogger();

		builder.Services.AddLocalization();

		builder.Services.AddDbContext<PersonsDbContext>(options =>
			options.UseSqlServer(configuration.GetConnectionString("TbcPersons")));

		builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
		builder.Services.AddMediatR(c =>
			c.RegisterServicesFromAssemblyContaining<Program>());

		builder.Services.AddTransient<ICityRepository, CityRepository>();
		builder.Services.AddTransient<IPersonRepository, PersonRepository>();
		builder.Services.AddTransient<IRelatedPersonRepository, RelatedPersonRepository>();

		builder.Services.AddScoped<ICityService, CityService>();
		builder.Services.AddScoped<IPersonService, PersonService>();
		builder.Services.AddScoped<IRelatedPersonService, RelatedPersonService>();

		return builder;
	}
}
