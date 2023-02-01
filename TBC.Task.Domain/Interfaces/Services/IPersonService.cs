namespace TBC.Task.Domain.Interfaces.Services;

public interface IPersonService : IService<Person>
{
	Person GetIncludeCity(int id);
}
