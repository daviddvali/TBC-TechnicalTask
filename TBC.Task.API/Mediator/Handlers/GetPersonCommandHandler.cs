using AutoMapper;
using MediatR;
using TBC.Task.API.Mediator.Commands;
using TBC.Task.API.Models;
using TBC.Task.Service.Interfaces.Services;

namespace TBC.Task.API.Mediator.Handlers;

public sealed class GetPersonCommandHandler : IRequestHandler<GetPersonQuery, ResponsePersonWithRelatedModel>
{
    private readonly IPersonService _personService;
    private readonly IRelatedPersonService _relatedPersonService;
    private readonly IMapper _mapper;

    public GetPersonCommandHandler(IPersonService personService, IRelatedPersonService relatedPersonService, IMapper mapper)
    {
        _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        _relatedPersonService = relatedPersonService ?? throw new ArgumentNullException(nameof(relatedPersonService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    public async Task<ResponsePersonWithRelatedModel> Handle(GetPersonQuery request, CancellationToken cancellationToken)
    {
        var person = BuildGetResponse(request.Id);

        return person!;
    }

    private ResponsePersonWithRelatedModel? BuildGetResponse(int id)
    {
        var person = _personService.GetIncludeCity(id);
        if (person == null)
            return null;

        var relatedPersons = _relatedPersonService.GetRelatedPersons(id);

        var response = _mapper.Map<ResponsePersonWithRelatedModel>(person);
        response.RelatedTo = relatedPersons.Select(p => _mapper.Map<ResponsePersonModel>(p));

        return response;
    }
}
