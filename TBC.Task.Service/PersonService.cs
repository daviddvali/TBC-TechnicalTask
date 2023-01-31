using TBC.Task.Domain;
using TBC.Task.Domain.Interfaces.Repositories;
using TBC.Task.Domain.Interfaces.Services;

namespace TBC.Task.Service;

public class PersonService : ServiceBase<Person, IPersonRepository>, IPersonService
{
	public PersonService(IPersonRepository repository) : base(repository) { }
}
