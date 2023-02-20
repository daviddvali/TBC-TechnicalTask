using AutoMapper;
using MediatR;
using TBC.Task.API.Mediator.Commands;
using TBC.Task.API.Models;
using TBC.Task.Domain;
using TBC.Task.Service.Interfaces.Services;

namespace TBC.Task.API.Mediator.Handlers;

public sealed class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, RequestPersonModel>
{
    private readonly IPersonService _personService;
    private readonly IMapper _mapper;

    public CreatePersonCommandHandler(IPersonService personService, IMapper mapper)
    {
        _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<RequestPersonModel> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = _mapper.Map<Person>(request.Model);
        person.Id = 0;
        _personService.Insert(person);

        return _mapper.Map<RequestPersonModel>(person);
    }
}
