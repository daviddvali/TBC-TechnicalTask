using TBC.Task.Domain.Enumerations;

namespace TBC.Task.API.Models;

public class ResponsePersonModel
{
	public int Id { get; init; }
	public string FirstName { get; init; } = null!;
	public string LastName { get; init; } = null!;
	public string PersonalNumber { get; init; } = null!;
	public GenderType? Gender { get; init; }
	public DateTime BirthDate { get; init; }
	public string? MobilePhone { get; init; }
	public string? WorkPhone { get; init; }
	public string? HomePhone { get; init; }
	public int? CityId { get; init; }
	public string? City { get; init; }
}
