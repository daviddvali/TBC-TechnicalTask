using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using AutoMapper;
using MediatR;
using TBC.Task.API.Localization;
using TBC.Task.API.Mediator.Requests.Commands;
using TBC.Task.API.Mediator.Requests.Queries;
using TBC.Task.API.Models;
using TBC.Task.API.Resources;
using TBC.Task.Service.Interfaces.Services;

namespace TBC.Task.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonsController : ControllerBase
{
    private readonly IPersonService _personService;
    private readonly IRelatedPersonService _relatedPersonService;
    private readonly IStringLocalizer<ErrorResources> _errorLocalizer;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public PersonsController(
        IPersonService personService,
        IRelatedPersonService relatedPersonService,
        IStringLocalizer<ErrorResources> errorLocalizer,
        IMapper mapper,
        IMediator mediator)
    {
        _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        _relatedPersonService = relatedPersonService ?? throw new ArgumentNullException(nameof(relatedPersonService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _errorLocalizer = errorLocalizer ?? throw new ArgumentNullException(nameof(errorLocalizer));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpPost]
    public async Task<IActionResult> Create(RequestPersonModel model)
    {
        var personModel = await _mediator.Send(new CreatePersonCommand(model));

        return Ok(new { id = personModel.Id });
    }

    [HttpPut]
    public async Task<IActionResult> Update(int id, RequestPersonModel model)
    {
        if (!_personService.Exists(id))
            return NotFound(_errorLocalizer.GetLocalized(ErrorResources.PersonNotFound));

        await _mediator.Send(new UpdatePersonCommand(model));

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!_personService.Exists(id))
            return NotFound(_errorLocalizer.GetLocalized(ErrorResources.PersonNotFound));

        await _mediator.Send(new DeletePersonCommand(id));

        return NoContent();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        if (!_personService.Exists(id))
            return NotFound(_errorLocalizer.GetLocalized(ErrorResources.PersonNotFound));

        var person = await _mediator.Send(new GetPersonQuery(id));

        return Ok(person);
    }

    [HttpPost]
    [Route("AddRelatedPerson/{from:int}/{to:int}")]
    public async Task<IActionResult> AddRelatedPerson(int from, int to)
    {
        if (_relatedPersonService.Exists(from, to))
            return Ok();

        await _mediator.Send(new CreateRelatedPersonCommand(new RequestRelatedPersonModel(from, to)));

        return Ok();
    }

    [HttpDelete]
    [Route("DeleteRelatedPerson/{from:int}/{to:int}")]
    public async Task<IActionResult> DeleteRelatedPerson(int from, int to)
    {
        if (!_relatedPersonService.Exists(from, to))
            return NotFound(_errorLocalizer.GetLocalized(ErrorResources.RelationNotFound));

        await _mediator.Send(new DeleteRelatedPersonCommand(new RequestRelatedPersonModel(from, to)));

        return NoContent();
    }

    [HttpGet]
    [Route("RelatedPersonsCount/{id:int}")]
    public async Task<IActionResult> GetRelatedPersonsCount(int id)
    {
        if (!_personService.Exists(id))
            return NotFound(_errorLocalizer.GetLocalized(ErrorResources.PersonNotFound));

        var count = await _mediator.Send(new GetRelatedPersonsCountQuery(id));

        return Ok(new { relatedPersonsCount = count });
    }

    [HttpPatch]
    [Route("UploadPhoto")]
    public async Task<IActionResult> UploadPhoto(int id, IFormFile file)
    {
        if (!_personService.Exists(id))
            return NotFound(_errorLocalizer.GetLocalized(ErrorResources.PersonNotFound));
        if (file.Length == 0)
            return BadRequest();

        await _mediator.Send(new RequestUploadPhotoModel(id, file));

        return Ok(new
        {
            id,
            photoSize = file.Length
        });
    }

    [HttpGet]
    [Route("Photo/{id:int}")]
    public async Task<IActionResult> GetPhoto(int id)
    {
        if (!_personService.Exists(id))
            return NotFound(_errorLocalizer.GetLocalized(ErrorResources.PersonNotFound));

        var data = await _mediator.Send(new GetPhotoQuery(id));

        if (!data.Any())
            return NotFound();

        return File(data, "image/jpeg");
    }

    [HttpGet]
    [Route("QuickSearch/{keyword}/{currentPage:int}/{pageSize:int?}")]
    public async Task<IActionResult> QuickSearch(string keyword, int currentPage = 1, int pageSize = 100)
    {
        var result = await _mediator.Send(new QuickSearchQuery(
            new RequestQuickSearchModel(keyword, currentPage, pageSize)));

        return Ok(result);
    }

    [HttpGet]
    [Route("Search/{keyword}/{currentPage:int?}/{pageSize:int?}")]
    public async Task<IActionResult> Search(
        string keyword, DateTime? birthDateFrom = null, DateTime? birthDateTo = null, int currentPage = 1, int pageSize = 100)
    {
        if (!(birthDateFrom.HasValue && birthDateTo.HasValue || !birthDateFrom.HasValue && !birthDateTo.HasValue))
            return new BadRequestObjectResult(_errorLocalizer.GetLocalized(ErrorResources.DateRangeNotSelected));

        var result = await _mediator.Send(new SearchQuery(
            new RequestSearchModel(keyword, birthDateFrom, birthDateTo, currentPage, pageSize)));

        return Ok(result);
    }
}
