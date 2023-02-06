namespace TBC.Task.API.Models;

public sealed record ResponseSearchModel(
	int CurrentPage,
	int PageSize,
	int ResultTotalCount,
	IEnumerable<ResponsePersonModel> Result)
{
	public int TotalPages => (int) Math.Ceiling((double) ResultTotalCount / PageSize);
}
