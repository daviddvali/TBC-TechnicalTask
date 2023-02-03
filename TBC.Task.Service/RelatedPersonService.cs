using TBC.Task.Domain;
using TBC.Task.Domain.Interfaces.Repositories;
using TBC.Task.Domain.Interfaces.Services;

namespace TBC.Task.Service;

public sealed class RelatedPersonService : ServiceBase<RelatedPerson, IRelatedPersonRepository>, IRelatedPersonService
{
	public RelatedPersonService(IRelatedPersonRepository repository) : base(repository) { }
	
	public int GetRelatedPersonsCount(int id) => _repository.Set(x => x.FromId == id).Count();

	public IEnumerable<Person> GetRelatedPersons(int id) => _repository.GetRelatedPersons(id);
}
