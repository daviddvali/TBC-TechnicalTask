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
			.HasColumnType("char")
			.IsRequired();
		_modelBuilder.Entity<Person>()
			.Property(e => e.PhotoPath)
			.HasMaxLength(250)
			.HasColumnType("varchar");
		_modelBuilder.Entity<Person>()
			.Property(e => e.PhotoUrl)
			.HasMaxLength(250)
			.HasColumnType("varchar");
		_modelBuilder.Entity<Person>()
			.Property(e => e.BirthDate)
			.HasColumnType("date")
			.IsRequired();
		_modelBuilder.Entity<Person>(e => e.OwnsOne(c => c.ContactInfo)
			.Property(p => p.MobilePhone)
			.HasMaxLength(50)
			.HasColumnType("varchar"));
		_modelBuilder.Entity<Person>(e => e.OwnsOne(c => c.ContactInfo)
			.Property(p => p.WorkPhone)
			.HasMaxLength(50)
			.HasColumnType("varchar"));
		_modelBuilder.Entity<Person>(e => e.OwnsOne(c => c.ContactInfo)
			.Property(p => p.HomePhone)
			.HasMaxLength(50)
			.HasColumnType("varchar"));

		_modelBuilder.Entity<Person>()
			.HasIndex(e => new { e.FirstName, e.LastName, e.PersonalNumber });

		return true;
	}
}
