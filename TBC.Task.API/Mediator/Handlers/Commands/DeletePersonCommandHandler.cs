using MediatR;
using TBC.Task.API.Mediator.Requests.Commands;
using TBC.Task.Service.Interfaces.Services;

namespace TBC.Task.API.Mediator.Handlers.Commands;

public sealed class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, int>
{
    private readonly IPersonService _personService;

    public DeletePersonCommandHandler(IPersonService personService) =>
        _personService = personService ?? throw new ArgumentNullException(nameof(personService));

    public async Task<int> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
	    if (!_personService.Exists(request.Id))
		    throw new KeyNotFoundException(request.Id.ToString());

		var person = _personService.Get(request.Id);
        _personService.Delete(person);

        return request.Id;
    }
}
