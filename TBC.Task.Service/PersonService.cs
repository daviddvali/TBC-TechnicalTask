using TBC.Task.Domain;
using TBC.Task.Domain.Interfaces.Repositories;
using TBC.Task.Domain.Interfaces.Services;

namespace TBC.Task.Service;

public sealed class PersonService : ServiceBase<Person, IPersonRepository>, IPersonService
{
	private readonly IRelatedPersonRepository _relatedPersonRepository;

	public PersonService(IPersonRepository repository, IRelatedPersonRepository relatedPersonRepository) : base(repository)
	{
		_relatedPersonRepository = relatedPersonRepository;
	}

	public override void Delete(Person entity)
	{
		var relations = _relatedPersonRepository
			.Set(x => x.ToId == entity.Id || x.FromId == entity.Id);
		foreach (var relation in relations)
		{
			_relatedPersonRepository.Delete(relation);
		}
		base.Delete(entity);
	}

	public Person? GetIncludeCity(int id) => _repository.GetIncludeCity(id);

	public IEnumerable<Person> QuickSearch(string keyword, int currentPage, int pageSize) =>
		_repository.QuickSearch(keyword, currentPage, pageSize);

	public IEnumerable<Person> Search(string keyword, int currentPage, int pageSize) =>
		_repository.Search(keyword, currentPage, pageSize);

	public bool Exists(int id) => _repository.Set().Any(x => x.Id == id);
}
