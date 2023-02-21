using MediatR;

namespace TBC.Task.API.Mediator.Requests.Queries;

public sealed class GetRelatedPersonsCountQuery : IRequest<int>
{
    public GetRelatedPersonsCountQuery(int id) =>
        Id = id;

    public int Id { get; }
}
