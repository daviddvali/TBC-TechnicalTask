using AutoMapper;
using MediatR;
using TBC.Task.API.Mediator.Requests.Commands;
using TBC.Task.API.Models;
using TBC.Task.Domain;
using TBC.Task.Service.Interfaces.Services;

namespace TBC.Task.API.Mediator.Handlers.Commands;

public sealed class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, RequestPersonModel>
{
    private readonly IPersonService _personService;
    private readonly IMapper _mapper;

    public UpdatePersonCommandHandler(IPersonService personService, IMapper mapper)
    {
        _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<RequestPersonModel> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
	    if (!_personService.Exists(request.Model.Id!.Value))
		    throw new KeyNotFoundException(request.Model.Id!.Value.ToString());

		var person = _mapper.Map<Person>(request.Model);
        _personService.Update(person);

        return _mapper.Map<RequestPersonModel>(person);
    }
}
