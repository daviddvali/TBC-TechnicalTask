using TBC.Task.Domain;
using TBC.Task.Repository.Database;

namespace TBC.Task.Repository;

public class CityRepository : RepositoryBase<City>
{
	public CityRepository(PersonsDbContext context) : base(context) { }
}
