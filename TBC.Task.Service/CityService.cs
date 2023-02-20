using TBC.Task.Domain;
using TBC.Task.Service.Interfaces.Repositories;
using TBC.Task.Service.Interfaces.Services;

namespace TBC.Task.Service;

public sealed class CityService : ServiceBase<City, ICityRepository>, ICityService
{
	public CityService(ICityRepository repository) : base(repository) { }
}
