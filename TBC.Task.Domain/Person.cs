using TBC.Task.Domain.ComplexTypes;
using TBC.Task.Domain.Enumerations;

namespace TBC.Task.Domain;

public class Person
{
	public int Id { get; set; }
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public string PersonalNumber { get; set; } = null!;
	public GenderType? Gender { get; set; }
	public DateTime BirthDate { get; set; }
	public ContactInfo? ContactInfo { get; set; }
	public string? Photo { get; set; }
	public City? City { get; set; }

	public ICollection<RelatedPerson>? RelatedTo { get; set; }
	public ICollection<RelatedPerson>? RelatedFrom { get; set; }
}
