namespace TBC.Task.API.Models;

public class ResponsePersonWithRelatedModel : ResponsePersonModel
{
	public IEnumerable<ResponsePersonModel>? RelatedTo { get; set; }
}
