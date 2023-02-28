using MediatR;
using TBC.Task.API.Mediator.Requests.Queries;
using TBC.Task.Service.Interfaces.Services;

namespace TBC.Task.API.Mediator.Handlers.Queries;

public sealed class GetRelatedPersonsCountHandler : IRequestHandler<GetRelatedPersonsCountQuery, int>
{
	private readonly IPersonService _personService;
	private readonly IRelatedPersonService _relatedPersonService;

	public GetRelatedPersonsCountHandler(IPersonService personService, IRelatedPersonService relatedPersonService)
	{
		_personService = personService ?? throw new ArgumentNullException(nameof(personService));
		_relatedPersonService = relatedPersonService ?? throw new ArgumentNullException(nameof(relatedPersonService));
	}

	public async Task<int> Handle(GetRelatedPersonsCountQuery request, CancellationToken cancellationToken)
	{
		if (!_personService.Exists(request.Id))
			throw new KeyNotFoundException(request.Id.ToString());

		var count = _relatedPersonService.GetRelatedPersonsCount(request.Id);

		return count;
	}
}
