using MediatR;

namespace TBC.Task.API.Mediator.Commands;

public sealed class GetPhotoQuery : IRequest<byte[]>
{
    public GetPhotoQuery(int id) =>
        Id = id;

    public int Id { get; }
}
