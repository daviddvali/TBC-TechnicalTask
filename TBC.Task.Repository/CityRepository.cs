using TBC.Task.Domain;
using TBC.Task.Domain.Interfaces.Repositories;
using TBC.Task.Repository.Database;

namespace TBC.Task.Repository;

public sealed class CityRepository : RepositoryBase<City>, ICityRepository
{
	public CityRepository(PersonsDbContext context) : base(context) { }
}
