using TBC.Task.Domain;
using TBC.Task.Repository.Database;

namespace TBC.Task.Repository;

public class PersonRepository : RepositoryBase<Person>
{
	public PersonRepository(PersonsDbContext context) : base(context) { }
}
