using MediatR;
using TBC.Task.API.Models;

namespace TBC.Task.API.Mediator.Commands;

public sealed class GetPersonQuery : IRequest<ResponsePersonWithRelatedModel>
{
    public GetPersonQuery(int id) =>
        Id = id;

    public int Id { get; }
}
