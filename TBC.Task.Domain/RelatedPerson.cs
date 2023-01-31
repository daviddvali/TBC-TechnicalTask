namespace TBC.Task.Domain;

public class RelatedPerson
{
	public int FromId { get; set; }
	public int ToId { get; set; }

	public Person? From { get; set; }
	public Person? To { get; set; }
}