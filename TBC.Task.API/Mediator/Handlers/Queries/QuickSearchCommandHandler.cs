using AutoMapper;
using MediatR;
using TBC.Task.API.Mediator.Requests.Queries;
using TBC.Task.API.Models;
using TBC.Task.Service.Interfaces.Services;

namespace TBC.Task.API.Mediator.Handlers.Queries;

public sealed class QuickSearchCommandHandler : IRequestHandler<QuickSearchQuery, ResponseSearchModel>
{
    private readonly IPersonService _personService;
    private readonly IMapper _mapper;

    public QuickSearchCommandHandler(IPersonService personService, IMapper mapper)
    {
        _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ResponseSearchModel> Handle(QuickSearchQuery request, CancellationToken cancellationToken)
    {
        var (result, resultTotalCount) = _personService.QuickSearch(
            request.Model.Keyword,
            request.Model.CurrentPage,
            request.Model.PageSize);

        var response = new ResponseSearchModel(
            request.Model.CurrentPage,
            request.Model.PageSize,
            resultTotalCount,
            result.Select(p => _mapper.Map<ResponsePersonModel>(p)).ToArray());

        return response;
    }
}
