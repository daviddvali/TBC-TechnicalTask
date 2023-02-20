using TBC.Task.Domain;
using TBC.Task.Service.Interfaces.Repositories;
using TBC.Task.Repository.Database;

namespace TBC.Task.Repository;

public sealed class CityRepository : RepositoryBase<City>, ICityRepository
{
	public CityRepository(PersonsDbContext context) : base(context) { }
}
