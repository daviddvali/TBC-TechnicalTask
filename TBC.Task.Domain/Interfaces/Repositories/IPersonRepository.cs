namespace TBC.Task.Domain.Interfaces.Repositories;

public interface IPersonRepository : IRepository<Person>
{
	Person GetIncludeCity(int id);
}
