using MediatR;
using TBC.Task.API.Models;

namespace TBC.Task.API.Mediator.Requests.Commands;

public sealed class CreateRelatedPersonCommand : IRequest<RequestRelatedPersonModel>
{
    public CreateRelatedPersonCommand(RequestRelatedPersonModel model) =>
        Model = model ?? throw new ArgumentNullException(nameof(model));

    public RequestRelatedPersonModel Model { get; }
}
