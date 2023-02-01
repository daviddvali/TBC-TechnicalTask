namespace TBC.Task.Domain.Interfaces.Services;

public interface IRelatedPersonService : IService<RelatedPerson>
{
	int GetRelatedPersonsCount(int id);
	IEnumerable<Person> GetRelatedPersons(int id);
}
