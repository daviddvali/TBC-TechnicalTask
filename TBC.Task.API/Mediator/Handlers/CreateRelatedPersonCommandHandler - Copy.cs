using AutoMapper;
using MediatR;
using TBC.Task.API.Mediator.Commands.Persons;
using TBC.Task.API.Mediator.Commands.RelatedPersons;
using TBC.Task.API.Models;
using TBC.Task.Domain;
using TBC.Task.Service.Interfaces.Services;

namespace TBC.Task.API.Mediator.Handlers;

public sealed class DeleteRelatedPersonCommandHandler : IRequestHandler<CreateRelatedPersonCommand, RelatedPersonModel>
{
    private readonly IRelatedPersonService _relatedPersonService;

    public DeleteRelatedPersonCommandHandler(IRelatedPersonService relatedPersonService) =>
        _relatedPersonService = relatedPersonService ?? throw new ArgumentNullException(nameof(relatedPersonService));

    public async Task<RelatedPersonModel> Handle(CreateRelatedPersonCommand request, CancellationToken cancellationToken)
    {
        _relatedPersonService.Delete(request.Model.FromId, request.Model.ToId);

        return new RelatedPersonModel(request.Model.FromId, request.Model.ToId);
    }
}
