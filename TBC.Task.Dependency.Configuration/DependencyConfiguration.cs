using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TBC.Task.Domain;
using TBC.Task.Repository;

namespace TBC.Task.Dependency.Configuration;

public class DependencyConfiguration
{
	public static DependencyConfiguration Instance => new DependencyConfiguration();

	internal DependencyConfiguration() { }

	public void Execute(IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<PersonsDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("TbcPersons")));
		
		// Get all domain interfaces
		var interfaces = typeof(Person).Assembly.GetTypes()
		  .Where(t => t.IsInterface);
	}
}
