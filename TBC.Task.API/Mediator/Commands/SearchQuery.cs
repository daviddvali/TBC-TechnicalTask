using MediatR;
using TBC.Task.API.Models;

namespace TBC.Task.API.Mediator.Commands;

public sealed class SearchQuery : IRequest<ResponseSearchModel>
{
    public SearchQuery(RequestSearchModel model) =>
        Model = model;

    public RequestSearchModel Model { get; }
}
