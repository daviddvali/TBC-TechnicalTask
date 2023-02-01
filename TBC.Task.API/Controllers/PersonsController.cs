using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using TBC.Task.API.Models;
using TBC.Task.Domain;
using TBC.Task.Domain.Interfaces.Services;

namespace TBC.Task.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonsController : ControllerBase
{
	private readonly IPersonService _personService;
	private readonly IRelatedPersonService _relatedPersonService;
	private readonly IMapper _mapper;
	private readonly IHostEnvironment _environment;
	private readonly ILogger<PersonsController> _logger;

	public PersonsController(
		IPersonService personService,
		IRelatedPersonService relatedPersonService,
		IMapper mapper,
		IHostEnvironment environment,
		ILogger<PersonsController> logger)
	{
		_personService = personService ?? throw new ArgumentNullException(nameof(personService));
		_relatedPersonService = relatedPersonService ?? throw new ArgumentNullException(nameof(relatedPersonService));
		_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		_environment = environment ?? throw new ArgumentNullException(nameof(environment));
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	[HttpPost]
	public async Task<IActionResult> Create(RequestPersonModel model)
	{
		Person person = _mapper.Map<Person>(model);
		var id = _personService.Insert(person);

		return Ok(new { id });
	}

	[HttpPut]
	public IActionResult Update(int id, RequestPersonModel model)
	{
		Person person = _mapper.Map<Person>(model);
		person.Id = id;
		_personService.Update(person);

		return NoContent();
	}

	[HttpDelete("{id:int}")]
	public IActionResult Delete(int id)
	{
		_personService.Delete(id);

		return NoContent();
	}

	[HttpPost]
	[Route("AddRelatedPerson/{from:int}/{to:int}")]
	public ActionResult<IEnumerable<Person>> AddRelatedPerson(int from, int to)
	{
		RelatedPerson relatedPerson = new() { FromId = from, ToId = to };
		_relatedPersonService.Insert(relatedPerson);

		return NoContent();
	}

	[HttpDelete]
	[Route("DeleteRelatedPerson/{from:int}/{to:int}")]
	public ActionResult<IEnumerable<Person>> DeleteRelatedPerson(int from, int to)
	{
		var relatedPerson = _relatedPersonService
			.Set(x => x.FromId == from && x.ToId == to)
			.FirstOrDefault();

		if (relatedPerson == default)
		{
			return NotFound();
		}

		_relatedPersonService.Delete(relatedPerson);

		return NoContent();
	}

	[HttpGet]
	[Route("RelatedPersonsCount/{id:int}")]
	public ActionResult<int> GetRelatedPersonsCount(int id)
	{
		int relatedPersonsCount = _relatedPersonService.GetRelatedPersonsCount(id);

		return Ok(new { relatedPersonsCount });
	}

	[HttpGet("{id:int}")]
	public ActionResult<Person?> Get(int id)
	{
		var person = BuildGetResponse(id);

		return Ok(person);
	}




	[HttpPost]
	[Route("UploadPhoto")]
	public ActionResult<IEnumerable<Person>> UploadPhoto(int id, IFormFile file)
	{
		if (file.Length == 0)
		{
			return Ok();
		}
		
		var person = _personService.Get(id);
		var imageDirectory = Path.Combine(_environment.ContentRootPath, "Uploads", "Photos", id.ToString());
		var imageFilePath = Path.Combine(imageDirectory, file.FileName);

		if (!Directory.Exists(imageDirectory))
		{
			Directory.CreateDirectory(imageDirectory);
		}

		using (var fileStream = System.IO.File.Create(Path.Combine(imageFilePath)))
		{
			file.CopyTo(fileStream);
			fileStream.Flush();
		}

		person.PhotoPath = imageFilePath;
		person.PhotoUrl = $"{Request.Scheme}://{Request.Host.Value}/Persons/GetPhoto/{person.Id}";
		
		_personService.Update(person);

		return Ok();
	}

	[HttpGet]
	[Route("GetPhoto/{id:int}")]
	public IActionResult GetPhoto(int id)
	{
		var person = _personService.Get(id);
		if (string.IsNullOrEmpty(person.PhotoPath))
		{
			return NoContent();
		}

		var data = System.IO.File.ReadAllBytes(person.PhotoPath);         
		return File(data, "image/jpeg");
	}

	[HttpGet]
	[Route("QuickSearch/{keyword}/{currentPage:int}/{pageSize:int?}")]
	public ActionResult<IEnumerable<Person>> QuickSearch(int keyword, int currentPage, int pageSize = 10)
	{

		return Ok();
	}

	[HttpGet]
	[Route("Search/{keyword}/{currentPage:int}/{pageSize:int?}")]
	public ActionResult<IEnumerable<Person>> Search(int keyword, int currentPage, int pageSize = 10)
	{

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

	#endregion
}
