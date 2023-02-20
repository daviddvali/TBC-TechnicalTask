using Microsoft.EntityFrameworkCore;
using TBC.Task.Domain;
using TBC.Task.Service.Interfaces.Repositories;
using TBC.Task.Repository.Database;

namespace TBC.Task.Repository;

public sealed class RelatedPersonRepository : RepositoryBase<RelatedPerson>, IRelatedPersonRepository
{
	public RelatedPersonRepository(PersonsDbContext context) : base(context) { }

	public IEnumerable<Person> GetRelatedPersons(int id) => _dbSet
		.Where(x => x.FromId == id)
		.Include(x => x.To)
		.Select(x => x.To)!;
}
