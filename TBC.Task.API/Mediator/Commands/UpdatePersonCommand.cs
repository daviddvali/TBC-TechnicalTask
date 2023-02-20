using MediatR;
using TBC.Task.API.Models;

namespace TBC.Task.API.Mediator.Commands;

public sealed class UpdatePersonCommand : IRequest<RequestPersonModel>
{
    public UpdatePersonCommand(RequestPersonModel model) =>
        Model = model ?? throw new ArgumentNullException(nameof(model));

    public RequestPersonModel Model { get; }
}
