using Microsoft.EntityFrameworkCore;
using TBC.Task.Domain;
using TBC.Task.Repository.Database.Interfaces;

namespace TBC.Task.Repository.Database.Configurations.Domains;

internal sealed class CityConfiguration : IEntityConfiguration
{
	private readonly ModelBuilder _modelBuilder;

	public CityConfiguration(ModelBuilder modelBuilder) =>
		_modelBuilder = modelBuilder ?? throw new ArgumentNullException(nameof(modelBuilder));

	public bool Configure()
	{
		_modelBuilder.Entity<City>()
			.HasKey(e => e.Id);

		_modelBuilder.Entity<City>()
			.Property(e => e.Name)
			.HasMaxLength(30)
			.IsRequired();

		return true;
	}
}
