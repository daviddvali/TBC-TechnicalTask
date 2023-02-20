using TBC.Task.Domain.Interfaces.Entities;

namespace TBC.Task.Domain.Interfaces.Services;

public interface IPersonService : IQueryService<Person>, ICommandService<Person>
{
	Person? GetIncludeCity(int id);
	(IQueryable<Person>, int) QuickSearch(string keyword, int currentPage, int pageSize);
	(IQueryable<Person>, int) Search(string keyword, DateTime? birthDateFrom, DateTime? birthDateTo, int currentPage, int pageSize);
	bool Exists(int id);
}
