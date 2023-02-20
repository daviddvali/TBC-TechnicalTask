using MediatR;
using TBC.Task.API.Mediator.Commands.RelatedPersons;
using TBC.Task.API.Models;
using TBC.Task.Domain;
using TBC.Task.Service.Interfaces.Services;

namespace TBC.Task.API.Mediator.Handlers;

public sealed class CreateRelatedPersonCommandHandler : IRequestHandler<CreateRelatedPersonCommand, RelatedPersonModel>
{
    private readonly IRelatedPersonService _relatedPersonService;

    public CreateRelatedPersonCommandHandler(IRelatedPersonService relatedPersonService) =>
        _relatedPersonService = relatedPersonService ?? throw new ArgumentNullException(nameof(relatedPersonService));

    public async Task<RelatedPersonModel> Handle(CreateRelatedPersonCommand request, CancellationToken cancellationToken)
    {
        var relatedPerson = new RelatedPerson { FromId = request.Model.FromId, ToId = request.Model.ToId };
        _relatedPersonService.Insert(relatedPerson);

        return new RelatedPersonModel(relatedPerson.FromId, relatedPerson.ToId);
    }
}
