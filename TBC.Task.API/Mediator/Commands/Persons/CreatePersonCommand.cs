﻿using MediatR;
using TBC.Task.API.Models;

namespace TBC.Task.API.Mediator.Commands.Persons;

public sealed class CreatePersonCommand : IRequest<RequestPersonModel>
{
    public CreatePersonCommand(RequestPersonModel model) => 
        Model = model ?? throw new ArgumentNullException(nameof(model));

    public RequestPersonModel Model { get; }
}