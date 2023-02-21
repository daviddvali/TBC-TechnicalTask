using MediatR;

namespace TBC.Task.API.Mediator.Requests.Queries;

public sealed class GetPhotoQuery : IRequest<byte[]>
{
    public GetPhotoQuery(int id) =>
        Id = id;

    public int Id { get; }
}
