using MediatR;
using TBC.Task.API.Mediator.Requests.Commands;
using TBC.Task.Service.Interfaces.Services;

namespace TBC.Task.API.Mediator.Handlers.Commands;

public sealed class UploadPhotoCommandHandler : IRequestHandler<UploadPhotoCommand, int>
{
    private readonly IPersonService _personService;
    private readonly IHostEnvironment _environment;
    private readonly IConfiguration _configuration;

    public UploadPhotoCommandHandler(IPersonService personService, IHostEnvironment environment, IConfiguration configuration)
    {
        _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<int> Handle(UploadPhotoCommand request, CancellationToken cancellationToken)
    {
	    if (!_personService.Exists(request.Model.Id))
		    throw new KeyNotFoundException(request.Model.Id.ToString());

		var person = _personService.Get(request.Model.Id);
        var imageFilePath = SavePhoto(request.Model.Id, request.Model.File);

        person.PhotoPath = imageFilePath;
        person.PhotoUrl = $"{_configuration["PhotoRelativeUrl"]}/{person.Id}";

        _personService.Update(person);

        return person.Id;
    }

    private string SavePhoto(int id, IFormFile file)
    {
        var imageDirectory = Path.Combine(_environment.ContentRootPath, "Uploads", "Photos", id.ToString());
        var imageFilePath = Path.Combine(imageDirectory, file.FileName);

        if (!Directory.Exists(imageDirectory))
            Directory.CreateDirectory(imageDirectory);

        using var stream = file.OpenReadStream();
        stream.Seek(0, SeekOrigin.Begin);

        using var fileStream = File.Create(Path.Combine(imageFilePath));
        stream.CopyTo(fileStream);
        fileStream.Flush();

        return imageFilePath;
    }
}
