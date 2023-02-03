using TBC.Task.Domain;
using TBC.Task.Domain.Interfaces.Repositories;
using TBC.Task.Domain.Interfaces.Services;

namespace TBC.Task.Service;

public class PersonService : ServiceBase<Person, IPersonRepository>, IPersonService
{
	public PersonService(IPersonRepository repository) : base(repository) { }

	public Person GetIncludeCity(int id) => _repository.GetIncludeCity(id);

	public IEnumerable<Person> QuickSearch(string keyword, int currentPage, int pageSize) =>
		_repository.QuickSearch(keyword, currentPage, pageSize);

	public IEnumerable<Person> Search(string keyword, int currentPage, int pageSize) =>
		_repository.Search(keyword, currentPage, pageSize);
}
