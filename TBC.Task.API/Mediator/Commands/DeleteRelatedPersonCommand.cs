using MediatR;
using TBC.Task.API.Models;

namespace TBC.Task.API.Mediator.Commands;

public sealed class DeleteRelatedPersonCommand : IRequest<RequestRelatedPersonModel>
{
    public DeleteRelatedPersonCommand(RequestRelatedPersonModel model) =>
        Model = model ?? throw new ArgumentNullException(nameof(model));

    public RequestRelatedPersonModel Model { get; }
}
