using TBC.Task.Domain.ComplexTypes;
using TBC.Task.Domain.Enumerations;
using TBC.Task.Domain.Interfaces.Entities;

namespace TBC.Task.Domain;

public class Person : IEntitiy
{
	public int Id { get; set; }
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public string PersonalNumber { get; set; } = null!;
	public GenderType? Gender { get; set; }
	public DateTime BirthDate { get; set; }
	public ContactInfo? ContactInfo { get; set; }
	public string? Photo { get; set; }
	public int? CityId { get; set; }
	public City? City { get; set; }

	public ICollection<RelatedPerson>? RelatedTo { get; set; }
	public ICollection<RelatedPerson>? RelatedFrom { get; set; }
}
