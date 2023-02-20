namespace TBC.Task.Domain.Interfaces.Services;

public interface IRelatedPersonService : IQueryService<RelatedPerson>, ICommandService<RelatedPerson>
{
	void Delete(int fromId, int toId);
	int GetRelatedPersonsCount(int id);
	IEnumerable<Person> GetRelatedPersons(int id);
	bool Exists(int fromId, int toId);
}
