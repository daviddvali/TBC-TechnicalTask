using MediatR;
using TBC.Task.API.Mediator.Requests.Commands;
using TBC.Task.API.Models;
using TBC.Task.Service.Interfaces.Services;

namespace TBC.Task.API.Mediator.Handlers.Commands;

public sealed class DeleteRelatedPersonCommandHandler : IRequestHandler<DeleteRelatedPersonCommand, RequestRelatedPersonModel>
{
    private readonly IRelatedPersonService _relatedPersonService;

    public DeleteRelatedPersonCommandHandler(IRelatedPersonService relatedPersonService) =>
        _relatedPersonService = relatedPersonService ?? throw new ArgumentNullException(nameof(relatedPersonService));

    public async Task<RequestRelatedPersonModel> Handle(DeleteRelatedPersonCommand request, CancellationToken cancellationToken)
    {
        _relatedPersonService.Delete(request.Model.FromId, request.Model.ToId);

        return new RequestRelatedPersonModel(request.Model.FromId, request.Model.ToId);
    }
}
