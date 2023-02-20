using MediatR;
using TBC.Task.API.Mediator.Commands;
using TBC.Task.API.Models;
using TBC.Task.Service.Interfaces.Services;

namespace TBC.Task.API.Mediator.Handlers;

public sealed class DeleteRelatedPersonCommandHandler : IRequestHandler<CreateRelatedPersonCommand, RequestRelatedPersonModel>
{
    private readonly IRelatedPersonService _relatedPersonService;

    public DeleteRelatedPersonCommandHandler(IRelatedPersonService relatedPersonService) =>
        _relatedPersonService = relatedPersonService ?? throw new ArgumentNullException(nameof(relatedPersonService));

    public async Task<RequestRelatedPersonModel> Handle(CreateRelatedPersonCommand request, CancellationToken cancellationToken)
    {
        _relatedPersonService.Delete(request.Model.FromId, request.Model.ToId);

        return new RequestRelatedPersonModel(request.Model.FromId, request.Model.ToId);
    }
}
