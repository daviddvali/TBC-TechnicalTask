using TBC.Task.Domain;
using TBC.Task.Repository.Database;

namespace TBC.Task.Repository;

public class CityRepository : Repository<City>
{
	public CityRepository(PersonsDbContext context) : base(context) { }
}
