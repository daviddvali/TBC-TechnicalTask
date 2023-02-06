namespace TBC.Task.Domain.Interfaces.Repositories;

public interface IPersonRepository : IRepository<Person>
{
	Person? GetIncludeCity(int id);
	(IQueryable<Person>, int) QuickSearch(string keyword, int currentPage, int pageSize);
	(IQueryable<Person>, int) Search(string keyword, DateTime? birthDateFrom, DateTime? birthDateTo, int currentPage, int pageSize);
}
