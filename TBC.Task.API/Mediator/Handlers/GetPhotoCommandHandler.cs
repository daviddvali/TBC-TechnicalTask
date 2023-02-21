using AutoMapper;
using MediatR;
using TBC.Task.API.Mediator.Commands;
using TBC.Task.API.Models;
using TBC.Task.Domain;
using TBC.Task.Service.Interfaces.Services;

namespace TBC.Task.API.Mediator.Handlers;

public sealed class GetPhotoCommandHandler : IRequestHandler<GetPhotoQuery, byte[]>
{
    private readonly IPersonService _personService;

    public GetPhotoCommandHandler(IPersonService personService) =>
        _personService = personService ?? throw new ArgumentNullException(nameof(personService));

    public async Task<byte[]> Handle(GetPhotoQuery request, CancellationToken cancellationToken)
    {
        var person = _personService.Get(request.Id);
        var data = await GetPhotoData(person);

        return data!;
    }

    private static async Task<byte[]> GetPhotoData(Person person)
    {
        if (string.IsNullOrEmpty(person.PhotoPath) || !System.IO.File.Exists(person.PhotoPath))
            return Array.Empty<byte>();

        return await System.IO.File.ReadAllBytesAsync(person.PhotoPath);
    }
}
