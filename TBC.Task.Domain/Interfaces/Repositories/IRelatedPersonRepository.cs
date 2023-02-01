namespace TBC.Task.Domain.Interfaces.Repositories;

public interface IRelatedPersonRepository : IRepository<RelatedPerson>
{
	IEnumerable<Person> GetRelatedPersons(int id);
}
