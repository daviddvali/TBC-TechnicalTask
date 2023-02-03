using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using AutoMapper;
using TBC.Task.API.Models;
using TBC.Task.Domain;
using TBC.Task.Domain.Interfaces.Services;
using TBC.Task.API.Resources;

namespace TBC.Task.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonsController : ControllerBase
{
	private readonly IPersonService _personService;
	private readonly IRelatedPersonService _relatedPersonService;
	private readonly IMapper _mapper;
	private readonly IHostEnvironment _environment;
	private readonly IStringLocalizer<ErrorResources> _errorLocalizer;
	private readonly ILogger<PersonsController> _logger;

	public PersonsController(
		IPersonService personService,
		IRelatedPersonService relatedPersonService,
		IMapper mapper,
		IHostEnvironment environment, 
		IStringLocalizer<ErrorResources> errorLocalizer,
		ILogger<PersonsController> logger)
	{
		_personService = personService ?? throw new ArgumentNullException(nameof(personService));
		_relatedPersonService = relatedPersonService ?? throw new ArgumentNullException(nameof(relatedPersonService));
		_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		_environment = environment ?? throw new ArgumentNullException(nameof(environment));
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_errorLocalizer = errorLocalizer ?? throw new ArgumentNullException(nameof(errorLocalizer));
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
		var person = _mapper.Map<Person>(model);
		person.Id = id;
		_personService.Update(person);

		return NoContent();
	}

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		_personService.Delete(id);

		return NoContent();
	}

	[HttpPost]
	[Route("AddRelatedPerson/{from:int}/{to:int}")]
	public async Task<IActionResult> AddRelatedPerson(int from, int to)
	{
		_relatedPersonService.Insert(new RelatedPerson { FromId = from, ToId = to });

		return NoContent();
	}

	[HttpDelete]
	[Route("DeleteRelatedPerson/{from:int}/{to:int}")]
	public async Task<IActionResult> DeleteRelatedPerson(int from, int to)
	{
		var relatedPerson = _relatedPersonService
			.Set(x => x.FromId == from && x.ToId == to)
			.FirstOrDefault();

		if (relatedPerson == null)
		{
			return NotFound();
		}

		_relatedPersonService.Delete(relatedPerson);

		return NoContent();
	}

	[HttpGet]
	[Route("RelatedPersonsCount/{id:int}")]
	public async Task<IActionResult> GetRelatedPersonsCount(int id)
	{
		var relatedPersonsCount = _relatedPersonService.GetRelatedPersonsCount(id);

		return Ok(new { relatedPersonsCount });
	}

	[HttpGet("{id:int}")]
	public async Task<IActionResult> Get(int id)
	{
		var person = BuildGetResponse(id);

		return Ok(person);
	}
	
	[HttpPost]
	[Route("UploadPhoto")]
	public async Task<IActionResult> UploadPhoto(int id, IFormFile file)
	{
		if (file.Length == 0)
		{
			return Ok();
		}
		
		var person = _personService.Get(id);
		var imageFilePath = SavePhoto(id, file);

		person.PhotoPath = imageFilePath;
		person.PhotoUrl = $"/Persons/Photo/{person.Id}";
		
		_personService.Update(person);

		return NoContent();
	}

	[HttpGet]
	[Route("Photo/{id:int}")]
	public async Task<IActionResult> GetPhoto(int id)
	{
		string text = _errorLocalizer["Hello"];

		var person = _personService.Get(id);
		if (string.IsNullOrEmpty(person.PhotoPath))
		{
			return NoContent();
		}

		var data = await System.IO.File.ReadAllBytesAsync(person.PhotoPath);         
		return File(data, "image/jpeg");
	}

	[HttpGet]
	[Route("QuickSearch/{keyword}/{currentPage:int}/{pageSize:int?}")]
	public async Task<ActionResult<IEnumerable<Person>>> QuickSearch(string keyword, int currentPage, int pageSize = 10)
	{
		var result = _personService.QuickSearch(keyword, currentPage, pageSize)
			.Select(p => _mapper.Map<ResponsePersonModel>(p));

		return Ok(result);
	}

	[HttpGet]
	[Route("Search/{keyword}/{currentPage:int}/{pageSize:int?}")]
	public async Task<ActionResult<IEnumerable<Person>>> Search(string keyword, int currentPage, int pageSize = 10)
	{
		var result = _personService.QuickSearch(keyword, currentPage, pageSize)
			.Select(p => _mapper.Map<ResponsePersonModel>(p));

		return Ok();
	}

	#region Private helper methods

	private ResponsePersonWithRelatedModel BuildGetResponse(int id)
	{
		var person = _personService.GetIncludeCity(id);
		var relatedPersons = _relatedPersonService.GetRelatedPersons(id);

		var response = _mapper.Map<ResponsePersonWithRelatedModel>(person);
		response.RelatedTo = relatedPersons.Select(p => _mapper.Map<ResponsePersonModel>(p));

		return response;
	}

	private string SavePhoto(int id, IFormFile file)
	{
		var imageDirectory = Path.Combine(_environment.ContentRootPath, "Uploads", "Photos", id.ToString());
		var imageFilePath = Path.Combine(imageDirectory, file.FileName);

		if (!Directory.Exists(imageDirectory))
		{
			Directory.CreateDirectory(imageDirectory);
		}

		using var fileStream = System.IO.File.Create(Path.Combine(imageFilePath));
		file.CopyTo(fileStream);
		fileStream.Flush();

		return imageFilePath;
	}

	#endregion
}
