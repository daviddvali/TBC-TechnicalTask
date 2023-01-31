using TBC.Task.Domain;
using TBC.Task.Repository.Database;

namespace TBC.Task.Repository;

public class RelatedPersonRepository : RepositoryBase<RelatedPerson>
{
	public RelatedPersonRepository(PersonsDbContext context) : base(context) { }
}
