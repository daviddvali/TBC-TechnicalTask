using TBC.Task.Domain;
using TBC.Task.Domain.Interfaces.Repositories;
using TBC.Task.Repository.Database;

namespace TBC.Task.Repository;

public class PersonRepository : RepositoryBase<Person>, IPersonRepository
{
	public PersonRepository(PersonsDbContext context) : base(context) { }
}
