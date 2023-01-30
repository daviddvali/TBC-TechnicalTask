using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
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
			.HasKey(e => e.Id);

		_modelBuilder.Entity<Person>()
			.Property(e => e.FirstName)
			.HasMaxLength(50)
			.IsRequired();
		_modelBuilder.Entity<Person>()
			.Property(e => e.LastName)
			.HasMaxLength(50)
			.IsRequired();
		_modelBuilder.Entity<Person>()
			.Property(e => e.PersonalNumber)
			.HasMaxLength(11)
			.IsRequired();
		_modelBuilder.Entity<Person>()
			.Property(e => e.BirthDate)
			.IsRequired();
		_modelBuilder.Entity<Person>(e => e.OwnsOne(c => c.ContactInfo)
			.Property(e => e.MobilePhone)
			.HasMaxLength(50));		
		_modelBuilder.Entity<Person>(e => e.OwnsOne(c => c.ContactInfo)
			.Property(e => e.WorkPhone)
			.HasMaxLength(50));
		_modelBuilder.Entity<Person>(e => e.OwnsOne(c => c.ContactInfo)
			.Property(e => e.HomePhone)
			.HasMaxLength(50));

		return true;
	}
}
