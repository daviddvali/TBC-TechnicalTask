using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using AutoMapper;
using TBC.Task.API.Localization;
using TBC.Task.API.Models;
using TBC.Task.API.Resources;
using TBC.Task.Domain;
using TBC.Task.Domain.Interfaces.Services;

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
	private readonly IConfiguration _configuration;
	private readonly ILogger<PersonsController> _logger;

	public PersonsController(
		IPersonService personService,
		IRelatedPersonService relatedPersonService,
		IHostEnvironment environment,
		IStringLocalizer<ErrorResources> errorLocalizer,
		IMapper mapper,
		IConfiguration configuration,
		ILogger<PersonsController> logger)
	{
		_personService = personService ?? throw new ArgumentNullException(nameof(personService));
		_relatedPersonService = relatedPersonService ?? throw new ArgumentNullException(nameof(relatedPersonService));
		_environment = environment ?? throw new ArgumentNullException(nameof(environment));
		_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		_errorLocalizer = errorLocalizer ?? throw new ArgumentNullException(nameof(errorLocalizer));
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
	}

	[HttpPost]
	public async Task<IActionResult> Create(RequestPersonModel model)
	{
		var person = _mapper.Map<Person>(model);
		var id = _personService.Insert(person);

		return Ok(new { id });
	}

	[HttpPut]
	public async Task<IActionResult> Update(int id, RequestPersonModel model)
	{
		if (!_personService.Exists(id))
			return NotFound(_errorLocalizer.GetLocalized(ErrorResources.PersonNotFound));

		var person = _mapper.Map<Person>(model);
		person.Id = id;
		_personService.Update(person);

		return NoContent();
	}

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		if (!_personService.Exists(id))
			return NotFound(_errorLocalizer.GetLocalized(ErrorResources.PersonNotFound));

		_personService.Delete(id);

		return NoContent();
	}

	[HttpGet("{id:int}")]
	public async Task<IActionResult> Get(int id)
	{
		var person = BuildGetResponse(id);
		if (person == null)
			return NotFound(_errorLocalizer.GetLocalized(ErrorResources.PersonNotFound));

		return Ok(person);
	}

	[HttpPost]
	[Route("AddRelatedPerson/{from:int}/{to:int}")]
	public async Task<IActionResult> AddRelatedPerson(int from, int to)
	{
		if (_relatedPersonService.Exists(from, to))
			return Ok();

		_relatedPersonService.Insert(new RelatedPerson { FromId = from, ToId = to });

		return Ok();
	}

	[HttpDelete]
	[Route("DeleteRelatedPerson/{from:int}/{to:int}")]
	public async Task<IActionResult> DeleteRelatedPerson(int from, int to)
	{
		if (!_relatedPersonService.Exists(from, to))
			return NotFound(_errorLocalizer.GetLocalized(ErrorResources.RelationNotFound));

		_relatedPersonService.Delete(from, to);

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

		var person = _personService.Get(id);
		var imageFilePath = SavePhoto(id, file);

		person.PhotoPath = imageFilePath;
		person.PhotoUrl = $"{_configuration["PhotoRelativeUrl"]}/{person.Id}";

		_personService.Update(person);

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
	public async Task<ActionResult<IEnumerable<Person>>> QuickSearch(string keyword, int currentPage, int pageSize = 10)
	{
		var result = _personService
			.QuickSearch(keyword, currentPage, pageSize)
			.Select(p => _mapper.Map<ResponsePersonModel>(p))
			.ToArray();

		return Ok(new
		{
			currentPage,
			pageSize,
			totalPages = (int) Math.Ceiling((double) result.Count() / pageSize),
			result
		});
	}

	[HttpGet]
	[Route("Search/{keyword}/{currentPage:int}/{pageSize:int?}")]
	public async Task<ActionResult<IEnumerable<Person>>> Search(string keyword, int currentPage, int pageSize = 10)
	{
		var result = _personService
			.Search(keyword, currentPage, pageSize)
			.Select(p => _mapper.Map<ResponsePersonModel>(p))
			.ToArray();

		return Ok(new
		{
			currentPage,
			pageSize,
			totalPages = (int) Math.Ceiling((double) result.Count() / pageSize),
			result
		});
	}

	#region Private helper methods

	private ResponsePersonWithRelatedModel? BuildGetResponse(int id)
	{
		var person = _personService.GetIncludeCity(id);
		if (person == null)
			return null;

		var relatedPersons = _relatedPersonService.GetRelatedPersons(id);

		var response = _mapper.Map<ResponsePersonWithRelatedModel>(person);
		response.RelatedTo = relatedPersons.Select(p => _mapper.Map<ResponsePersonModel>(p));

		return response;
	}

	private static async Task<byte[]> GetPhotoData(Person person)
	{
		if (string.IsNullOrEmpty(person.PhotoPath) || !System.IO.File.Exists(person.PhotoPath))
			return Array.Empty<byte>();

		return await System.IO.File.ReadAllBytesAsync(person.PhotoPath);
	}

	private string SavePhoto(int id, IFormFile file)
	{
		var imageDirectory = Path.Combine(_environment.ContentRootPath, "Uploads", "Photos", id.ToString());
		var imageFilePath = Path.Combine(imageDirectory, file.FileName);

		if (!Directory.Exists(imageDirectory))
			Directory.CreateDirectory(imageDirectory);

		using var fileStream = System.IO.File.Create(Path.Combine(imageFilePath));
		file.CopyTo(fileStream);
		fileStream.Flush();

		return imageFilePath;
	}

	#endregion
}
