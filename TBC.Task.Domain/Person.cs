using TBC.Task.Domain.ComplexTypes;
using TBC.Task.Domain.Enumerations;

namespace TBC.Task.Domain;

public class Person
{
	public int Id { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public GenderType Gender { get; set; }
	public string PersonalNumber { get; set; }
	public DateTime BirthDate { get; set; }
	public City City { get; set; }
	//public ContactInfo ContactInfo { get; set; }
	public string Photo { get; set; }
	public ICollection<RelatedPerson> RelatedPersons { get; set; }
}
