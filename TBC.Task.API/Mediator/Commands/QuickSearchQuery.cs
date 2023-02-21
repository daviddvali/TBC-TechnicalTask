using MediatR;
using TBC.Task.API.Models;

namespace TBC.Task.API.Mediator.Commands;

public sealed class QuickSearchQuery : IRequest<ResponseSearchModel>
{
    public QuickSearchQuery(RequestQuickSearchModel model) =>
        Model = model;

    public RequestQuickSearchModel Model { get; }
}
