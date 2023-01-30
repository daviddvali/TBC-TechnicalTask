using Microsoft.EntityFrameworkCore;
using TBC.Task.Domain;
using TBC.Task.Repository.Database.Configurations;

namespace TBC.Task.Repository;

public class PersonsDbContext : DbContext
{
	public PersonsDbContext(DbContextOptions<PersonsDbContext> options) : base(options)
	{

	}

	public DbSet<Person> Persons { get; set; }
	public DbSet<City> Cities { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		DomainConfigurationManager.Execute(modelBuilder);
	}
}