namespace TBC.Task.API.Models;

public sealed class ResponsePersonWithRelatedModel : ResponsePersonModel
{
	public IEnumerable<ResponsePersonModel>? RelatedTo { get; set; }
}
