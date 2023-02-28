using MediatR;
using TBC.Task.API.Mediator.Requests.Commands;
using TBC.Task.API.Models;
using TBC.Task.Domain;
using TBC.Task.Service.Interfaces.Services;

namespace TBC.Task.API.Mediator.Handlers.Commands;

public sealed class CreateRelatedPersonCommandHandler : IRequestHandler<CreateRelatedPersonCommand, RequestRelatedPersonModel>
{
    private readonly IRelatedPersonService _relatedPersonService;

    public CreateRelatedPersonCommandHandler(IRelatedPersonService relatedPersonService) =>
        _relatedPersonService = relatedPersonService ?? throw new ArgumentNullException(nameof(relatedPersonService));

    public async Task<RequestRelatedPersonModel> Handle(CreateRelatedPersonCommand request, CancellationToken cancellationToken)
    {
	    if (_relatedPersonService.Exists(request.Model.FromId, request.Model.ToId))
		    return new RequestRelatedPersonModel(request.Model.FromId, request.Model.ToId);

		var relatedPerson = new RelatedPerson { FromId = request.Model.FromId, ToId = request.Model.ToId };
        _relatedPersonService.Insert(relatedPerson);

        return new RequestRelatedPersonModel(relatedPerson.FromId, relatedPerson.ToId);
    }
}
