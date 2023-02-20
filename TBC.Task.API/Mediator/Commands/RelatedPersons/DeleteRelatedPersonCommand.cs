using MediatR;
using TBC.Task.API.Models;

namespace TBC.Task.API.Mediator.Commands.RelatedPersons;

public sealed class DeleteRelatedPersonCommand : IRequest<RelatedPersonModel>
{
    public DeleteRelatedPersonCommand(RelatedPersonModel model) =>
        Model = model ?? throw new ArgumentNullException(nameof(model));

    public RelatedPersonModel Model { get; }
}
