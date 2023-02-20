using MediatR;

namespace TBC.Task.API.Mediator.Commands;

public sealed class DeletePersonCommand : IRequest<int>
{
    public DeletePersonCommand(int id) =>
        Id = id;

    public int Id { get; }
}
