using TBC.Task.Domain;
using TBC.Task.Domain.Interfaces.Repositories;
using TBC.Task.Domain.Interfaces.Services;

namespace TBC.Task.Service;

public sealed class RelatedPersonService : ServiceBase<RelatedPerson, IRelatedPersonRepository>, IRelatedPersonService
{
	public RelatedPersonService(IRelatedPersonRepository repository) : base(repository) { }

	public void Delete(int fromId, int toId) => Delete(Get(fromId, toId));

	public int GetRelatedPersonsCount(int id) => _repository.Set(x => x.FromId == id).Count();

	public IEnumerable<Person> GetRelatedPersons(int id) => _repository.GetRelatedPersons(id);

	public bool Exists(int fromId, int toId) => _repository.Set().Any(x => x.FromId == fromId && x.ToId == toId);
}
