using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using AutoMapper;
using MediatR;
using TBC.Task.API.Localization;
using TBC.Task.API.Mediator.Commands;
using TBC.Task.API.Models;
using TBC.Task.API.Resources;
using TBC.Task.Domain;
using TBC.Task.Service.Interfaces.Services;

namespace TBC.Task.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonsController : ControllerBase
{
    private readonly IPersonService _personService;
    private readonly IRelatedPersonService _relatedPersonService;
    private readonly IHostEnvironment _environment;
    private readonly IStringLocalizer<ErrorResources> _errorLocalizer;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;

    public PersonsController(
        IPersonService personService,
        IRelatedPersonService relatedPersonService,
        IHostEnvironment environment,
        IStringLocalizer<ErrorResources> errorLocalizer,
        IMapper mapper,
        IMediator mediator,
        IConfiguration configuration)
    {
        _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        _relatedPersonService = relatedPersonService ?? throw new ArgumentNullException(nameof(relatedPersonService));
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _errorLocalizer = errorLocalizer ?? throw new ArgumentNullException(nameof(errorLocalizer));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
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

        return Ok(new
        {
            relatedPersonsCount = _relatedPersonService.GetRelatedPersonsCount(id)
        });
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

        var person = _personService.Get(id);
        var data = await GetPhotoData(person);

        if (!data.Any())
            return NotFound();

        return File(data, "image/jpeg");
    }

    [HttpGet]
    [Route("QuickSearch/{keyword}/{currentPage:int}/{pageSize:int?}")]
    public async Task<IActionResult> QuickSearch(
        string keyword, int currentPage = 1, int pageSize = 100)
    {
        var (result, resultTotalCount) = _personService.QuickSearch(keyword, currentPage, pageSize);

        return Ok(new ResponseSearchModel(
            currentPage,
            pageSize,
            resultTotalCount,
            result
                .Select(p => _mapper.Map<ResponsePersonModel>(p))
                .ToArray()
        ));
    }

    [HttpGet]
    [Route("Search/{keyword}/{currentPage:int?}/{pageSize:int?}")]
    public async Task<IActionResult> Search(
        string keyword, DateTime? birthDateFrom = null, DateTime? birthDateTo = null, int currentPage = 1, int pageSize = 100)
    {
        if (!(birthDateFrom.HasValue && birthDateTo.HasValue || !birthDateFrom.HasValue && !birthDateTo.HasValue))
            return new BadRequestObjectResult(_errorLocalizer.GetLocalized(ErrorResources.DateRangeNotSelected));

        var (result, resultTotalCount) = _personService.Search(keyword, birthDateFrom, birthDateTo, currentPage, pageSize);

        return Ok(new ResponseSearchModel(
            currentPage,
            pageSize,
            resultTotalCount,
            result
                .Select(p => _mapper.Map<ResponsePersonModel>(p))
                .ToArray()
        ));
    }

    #region Private helper methods

    private static async Task<byte[]> GetPhotoData(Person person)
    {
        if (string.IsNullOrEmpty(person.PhotoPath) || !System.IO.File.Exists(person.PhotoPath))
            return Array.Empty<byte>();

        return await System.IO.File.ReadAllBytesAsync(person.PhotoPath);
    }

    #endregion
}
