using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TBC.Task.Domain;
using TBC.Task.Domain.ComplexTypes;
using TBC.Task.Repository.Database.Interfaces;

namespace TBC.Task.Repository.Database.Configurations.Domains;

internal class CityConfiguration : IEntityConfiguration
{
	private ModelBuilder _modelBuilder;

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
