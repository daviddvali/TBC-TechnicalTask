using MediatR;
using TBC.Task.API.Mediator.Commands;
using TBC.Task.Service.Interfaces.Services;

namespace TBC.Task.API.Mediator.Handlers;

public sealed class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, int>
{
    private readonly IPersonService _personService;

    public DeletePersonCommandHandler(IPersonService personService) => 
        _personService = personService ?? throw new ArgumentNullException(nameof(personService));

    public async Task<int> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        var person = _personService.Get(request.Id);
        _personService.Delete(person);

        return request.Id;
    }
}
