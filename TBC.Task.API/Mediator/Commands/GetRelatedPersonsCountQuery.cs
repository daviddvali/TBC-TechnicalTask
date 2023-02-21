using MediatR;

namespace TBC.Task.API.Mediator.Commands;

public sealed class GetRelatedPersonsCountQuery : IRequest<int>
{
    public GetRelatedPersonsCountQuery(int id) =>
        Id = id;

    public int Id { get; }
}
