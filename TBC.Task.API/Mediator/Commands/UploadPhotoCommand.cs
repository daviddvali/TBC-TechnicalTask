using MediatR;
using TBC.Task.API.Models;

namespace TBC.Task.API.Mediator.Commands;

public sealed class UploadPhotoCommand : IRequest<int>
{
    public UploadPhotoCommand(RequestUploadPhotoModel model) =>
        Model = model ?? throw new ArgumentNullException(nameof(model));

    public RequestUploadPhotoModel Model { get; }
}
