using AutoMapper;
using MediatR;
using TBC.Task.API.Mediator.Commands;
using TBC.Task.API.Models;
using TBC.Task.Service.Interfaces.Services;

namespace TBC.Task.API.Mediator.Handlers;

public sealed class SearchCommandHandler : IRequestHandler<SearchQuery, ResponseSearchModel>
{
    private readonly IPersonService _personService;
    private readonly IMapper _mapper;

    public SearchCommandHandler(IPersonService personService, IMapper mapper)
    {
        _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ResponseSearchModel> Handle(SearchQuery request, CancellationToken cancellationToken)
    {
        var (result, resultTotalCount) = _personService.Search(
            request.Model.Keyword,
            request.Model.BirthDateFrom,
            request.Model.BirthDateTo,
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
