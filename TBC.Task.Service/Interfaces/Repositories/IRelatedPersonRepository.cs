using TBC.Task.Domain;

namespace TBC.Task.Service.Interfaces.Repositories;

public interface IRelatedPersonRepository : IRepository<RelatedPerson>
{
	IEnumerable<Person> GetRelatedPersons(int id);
}
