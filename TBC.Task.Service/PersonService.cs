using TBC.Task.Domain;
using TBC.Task.Domain.Interfaces.Repositories;
using TBC.Task.Domain.Interfaces.Services;
using System.Linq;

namespace TBC.Task.Service;

public class PersonService : ServiceBase<Person, IPersonRepository>, IPersonService
{
	public PersonService(IPersonRepository repository) : base(repository) { }

	public Person GetIncludeCity(int id) => _repository.GetIncludeCity(id);
}
