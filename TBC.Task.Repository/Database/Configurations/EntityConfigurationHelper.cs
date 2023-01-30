using Microsoft.EntityFrameworkCore;
using TBC.Task.Repository.Database.Interfaces;

namespace TBC.Task.Repository.Database.Configurations;

internal static class EntityConfigurationHelper
{
	public static void ConfigureEntities(this ModelBuilder modelBuilder)
	{
		var configurationType = typeof(IEntityConfiguration);
		_ = (
		  from t in typeof(IEntityConfiguration).Assembly.GetTypes()
		  where configurationType.IsAssignableFrom(t) && !t.IsAbstract
		  select (Activator.CreateInstance(t, modelBuilder) as IEntityConfiguration)?.Configure()
		).ToArray();
	}
}
