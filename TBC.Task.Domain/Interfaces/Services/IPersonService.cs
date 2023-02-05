namespace TBC.Task.Domain.Interfaces.Services;

public interface IPersonService : IService<Person>
{
	Person? GetIncludeCity(int id);
	IEnumerable<Person> QuickSearch(string keyword, int currentPage, int pageSize);
	IEnumerable<Person> Search(string keyword, int currentPage, int pageSize);
	bool Exists(int id);
}
