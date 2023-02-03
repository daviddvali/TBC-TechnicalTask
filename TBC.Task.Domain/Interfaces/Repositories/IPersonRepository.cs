namespace TBC.Task.Domain.Interfaces.Repositories;

public interface IPersonRepository : IRepository<Person>
{
	Person GetIncludeCity(int id);
	IQueryable<Person> QuickSearch(string keyword, int currentPage, int pageSize);
	IQueryable<Person> Search(string keyword, int currentPage, int pageSize);
}
