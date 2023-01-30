namespace TBC.Task.Domain;

public class RelatedPerson
{
	public int Id { get; set; }
	public Person Source { get; set; }
	public Person Destination { get; set; }
}