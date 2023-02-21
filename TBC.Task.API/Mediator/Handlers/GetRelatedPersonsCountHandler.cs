using MediatR;
using TBC.Task.API.Mediator.Commands;
using TBC.Task.Service.Interfaces.Services;

namespace TBC.Task.API.Mediator.Handlers;

public sealed class GetRelatedPersonsCountHandler : IRequestHandler<GetRelatedPersonsCountQuery, int>
{
    private readonly IRelatedPersonService _relatedPersonService;

    public GetRelatedPersonsCountHandler(IRelatedPersonService relatedPersonService) =>
        _relatedPersonService = relatedPersonService ?? throw new ArgumentNullException(nameof(relatedPersonService));

    public async Task<int> Handle(GetRelatedPersonsCountQuery request, CancellationToken cancellationToken)
    {
        var count = _relatedPersonService.GetRelatedPersonsCount(request.Id);

        return count;
    }
}
