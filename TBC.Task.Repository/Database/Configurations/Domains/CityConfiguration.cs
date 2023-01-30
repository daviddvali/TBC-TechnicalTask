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

		//table.CheckConstraint("CK_City_Name", $"LEN({nameof(City.Name)}) >= 3");
		//table.CheckConstraint("CK_Person_FirstName", $"LEN({nameof(Person.FirstName)}) >= 2");
		//table.CheckConstraint("CK_Person_LastName", $"LEN({nameof(Person.LastName)}) >= 2");
		//table.CheckConstraint("CK_Person_PersonalNumber", $"LEN({nameof(Person.PersonalNumber)}) = 11");
		//table.CheckConstraint("CK_Person_ContactInfo_MobilePhone", $"LEN({nameof(ContactInfo)}_{nameof(ContactInfo.MobilePhone)}) >= 4");
		//table.CheckConstraint("CK_Person_ContactInfo_WorkPhone", $"LEN({nameof(ContactInfo)}_{nameof(ContactInfo.WorkPhone)}) >= 4");
		//table.CheckConstraint("CK_Person_ContactInfo_HomePhone", $"LEN({nameof(ContactInfo)}_{nameof(ContactInfo.HomePhone)}) >= 4");

		return true;
	}
}
