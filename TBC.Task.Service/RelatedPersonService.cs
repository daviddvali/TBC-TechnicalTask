using TBC.Task.Domain;
using TBC.Task.Domain.Interfaces.Repositories;
using TBC.Task.Domain.Interfaces.Services;

namespace TBC.Task.Service;

public class RelatedPersonService : ServiceBase<RelatedPerson, IRelatedPersonRepository>, IRelatedPersonService
{
	public RelatedPersonService(IRelatedPersonRepository repository) : base(repository) { }
}
