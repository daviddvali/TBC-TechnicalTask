using Microsoft.EntityFrameworkCore;
using TBC.Task.Domain;

namespace TBC.Task.Repository.Database.Configurations;

internal static class DatabaseInitializerExtensions
{
	public static void SeedDefaultData(this ModelBuilder modelBuilder) =>
		modelBuilder.Entity<City>().HasData(
			new City() { Id = 1, Name = "Tbilisi" },
			new City() { Id = 2, Name = "Batumi" },
			new City() { Id = 3, Name = "Kutaisi" },
			new City() { Id = 4, Name = "Rustavi" },
			new City() { Id = 5, Name = "Gori" }
		);
}
