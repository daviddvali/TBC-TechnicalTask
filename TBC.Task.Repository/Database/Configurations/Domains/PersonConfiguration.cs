using Microsoft.EntityFrameworkCore;
using TBC.Task.Domain;
using TBC.Task.Repository.Database.Interfaces;

namespace TBC.Task.Repository.Database.Configurations.Domains;

internal class PersonConfiguration : IEntityConfiguration
{
	private ModelBuilder _modelBuilder;

	public PersonConfiguration(ModelBuilder modelBuilder) =>
		_modelBuilder = modelBuilder ?? throw new ArgumentNullException(nameof(modelBuilder));

	public bool Configure()
	{
		_modelBuilder.Entity<Person>()
		  .HasKey(u => u.Id);

		return true;
	}
}
