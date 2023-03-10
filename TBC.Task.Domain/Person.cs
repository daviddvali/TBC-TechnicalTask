using TBC.Task.Domain.Enumerations;
using TBC.Task.Domain.Interfaces;

namespace TBC.Task.Domain;

public class Person : IEntitiy
{
	public int Id { get; set; }
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public string PersonalNumber { get; set; } = null!;
	public GenderType? Gender { get; set; }
	public DateTime BirthDate { get; set; }
	public string? MobilePhone { get; set; }
	public string? WorkPhone { get; set; }
	public string? HomePhone { get; set; }
	public string? PhotoPath { get; set; }
	public string? PhotoUrl { get; set; }
	public int? CityId { get; set; }
	public City? City { get; set; }

	public ICollection<RelatedPerson>? RelatedTo { get; set; }
	public ICollection<RelatedPerson>? RelatedFrom { get; set; }
}
