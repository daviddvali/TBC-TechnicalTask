using TBC.Task.Domain.Enumerations;

namespace TBC.Task.API.Models;

public record PersonModel
{
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public string PersonalNumber { get; set; } = null!;
	public GenderType? Gender { get; set; }
	public DateTime BirthDate { get; set; }
	public string? MobilePhone { get; set; }
	public string? WorkPhone { get; set; }
	public string? HomePhone { get; set; }
	public int? CityId { get; set; }
}
