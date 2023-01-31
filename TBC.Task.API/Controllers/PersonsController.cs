using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TBC.Task.API.Models;
using TBC.Task.Domain;
using TBC.Task.Domain.Interfaces.Services;

namespace TBC.Task.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonsController : ControllerBase
{
	private readonly ICityService _cityService;
	private readonly IPersonService _personService;
	private readonly IRelatedPersonService _relatedPersonService;
	private readonly IMapper _mapper;
	private readonly ILogger<PersonsController> _logger;

	public PersonsController(
		ICityService cityService,
		IPersonService personService,
		IRelatedPersonService relatedPersonService,
		IMapper mapper,
		ILogger<PersonsController> logger)
	{
		_cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
		_personService = personService ?? throw new ArgumentNullException(nameof(personService));
		_relatedPersonService = relatedPersonService ?? throw new ArgumentNullException(nameof(relatedPersonService));
		_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	[HttpGet("{id:int}")]
	public ActionResult<Person?> GetById(int id)
	{

		return null;
	}

	[HttpGet]
	[Route("RelatedCount/{id:int}")]
	public ActionResult<int> GetRelatedPersonsCount(int id)
	{

		return 1;
	}

	[HttpGet]
	[Route("Search/{keyword}/{currentPage:int}/{pageSize:int?}")]
	public ActionResult<IEnumerable<Person>> Search(int keyword, int currentPage, int pageSize = 10)
	{

		return Ok();
	}

	[HttpPost]
	public IActionResult Create(PersonModel model)
	{
		Person person = _mapper.Map<Person>(model);

		return Ok(new { id = 0 });
	}

	[HttpPut]
	public IActionResult Update(int id, PersonModel model)
	{

		return NoContent();
	}

	[HttpDelete("{id:int}")]
	public IActionResult Delete(int id)
	{

		return NoContent();
	}
}