using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TBC.Task.API.Models;
using TBC.Task.Domain.Interfaces.Services;

namespace TBC.Task.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonsController : ControllerBase
{
	private readonly ICityService _cityService;
	private readonly IPersonService _personService;
	private readonly IRelatedPersonService _relatedPersonService;
	private readonly ILogger<PersonsController> _logger;

	public PersonsController(
		ICityService cityService,
		IPersonService personService,
		IRelatedPersonService relatedPersonService,
		ILogger<PersonsController> logger)
	{
		_cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
		_personService = personService ?? throw new ArgumentNullException(nameof(personService));
		_relatedPersonService = relatedPersonService ?? throw new ArgumentNullException(nameof(relatedPersonService));
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	[HttpGet("{id}")]
	public ActionResult<int> GetById(int id)
	{

		return 1;
	}

	[HttpPost]
	public IActionResult Create(PersonModel model)
	{


		return Ok(new { id = 0 });
	}

	[HttpPut]
	public IActionResult Update(int id, PersonModel model)
	{

		return NoContent();
	}

	[HttpDelete("{id}")]
	public IActionResult Delete(int id)
	{

		return NoContent();
	}
}