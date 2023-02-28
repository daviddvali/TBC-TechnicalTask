using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MediatR;
using Polly;
using TBC.Task.API.Extensions;
using TBC.Task.API.Localization;
using TBC.Task.API.Mediator.Requests.Commands;
using TBC.Task.API.Mediator.Requests.Queries;
using TBC.Task.API.Models;
using TBC.Task.API.Resources;

namespace TBC.Task.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonsController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly IStringLocalizer<ErrorResources> _errorLocalizer;

	public PersonsController(IMediator mediator, IStringLocalizer<ErrorResources> errorLocalizer)
	{
		_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		_errorLocalizer = errorLocalizer ?? throw new ArgumentNullException(nameof(errorLocalizer));
	}

	internal IStringLocalizer<ErrorResources> ErrorLocalizer => _errorLocalizer;

	#region Person CRUD Actions

	[HttpPost]
	public async Task<IActionResult> Create(RequestPersonModel model) =>
		this.GetPersonPolicy().Execute(() =>
		{
			var id = _mediator.Send(new CreatePersonCommand(model)).Result.Id;
			return Ok(new { id });
		});

	[HttpPut]
	public async Task<IActionResult> Update(int id, RequestPersonModel model) =>
		this.GetPersonPolicy().Execute(() =>
		{
			model.Id = id;
			var _ = _mediator.Send(new UpdatePersonCommand(model)).Result;
			return NoContent();
		});

	[HttpPatch]
	[Route("UploadPhoto")]
	public async Task<IActionResult> UploadPhoto(int id, IFormFile file) =>
		this.GetPersonPolicy().Execute(() =>
		{
			if (file.Length == 0)
				return BadRequest();

			var _ = _mediator.Send(new UploadPhotoCommand(new RequestUploadPhotoModel(id, file))).Result;

			return Ok(new
			{
				id,
				photoSize = file.Length
			});
		});

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id) =>
		this.GetPersonPolicy().Execute(() =>
		{
			var _ = _mediator.Send(new DeletePersonCommand(id)).Result;
			return NoContent();
		});

	#endregion

	#region Related Person CRUD Actions

	[HttpPost]
	[Route("AddRelatedPerson/{from:int}/{to:int}")]
	public async Task<IActionResult> AddRelatedPerson(int from, int to) =>
		this.GetRelatedPersonPolicy().Execute(() =>
		{
			var _ = _mediator.Send(new CreateRelatedPersonCommand(new RequestRelatedPersonModel(from, to))).Result;
			return Ok();
		});

	[HttpDelete]
	[Route("DeleteRelatedPerson/{from:int}/{to:int}")]
	public async Task<IActionResult> DeleteRelatedPerson(int from, int to) =>
		this.GetRelatedPersonPolicy().Execute(() =>
		{
			var _ = _mediator.Send(new DeleteRelatedPersonCommand(new RequestRelatedPersonModel(from, to))).Result;
			return NoContent();
		});

	[HttpGet]
	[Route("RelatedPersonsCount/{id:int}")]
	public async Task<IActionResult> GetRelatedPersonsCount(int id) =>
		this.GetRelatedPersonPolicy().Execute(() =>
		{
			var count = _mediator.Send(new GetRelatedPersonsCountQuery(id)).Result;
			return Ok(new { relatedPersonsCount = count });
		});

	#endregion

	#region Person Query Actions

	[HttpGet("{id:int}")]
	public async Task<IActionResult> Get(int id) => Policy<IActionResult>
		.Handle<AggregateException>(_ => _.InnerException is KeyNotFoundException)
		.Fallback(_ => NotFound(_errorLocalizer.GetLocalized(ErrorResources.PersonNotFound)))
		.Execute(() => Ok(_mediator.Send(new GetPersonQuery(id)).Result));

	[HttpGet]
	[Route("Photo/{id:int}")]
	public async Task<IActionResult> GetPhoto(int id) =>
		this.GetPersonPolicy().Execute(() =>
		{
			var data = _mediator.Send(new GetPhotoQuery(id)).Result;

			if (!data.Any())
				return NotFound();

			return File(data, "image/jpeg");
		});

	[HttpGet]
	[Route("QuickSearch/{keyword}/{currentPage:int}/{pageSize:int?}")]
	public async Task<IActionResult> QuickSearch(string keyword, int currentPage = 1, int pageSize = 100) =>
		this.GetQueryPolicy().Execute(() =>
		{
			var result = _mediator.Send(new QuickSearchQuery(
				new RequestQuickSearchModel(keyword, currentPage, pageSize))).Result;

			return Ok(result);
		});

	[HttpGet]
	[Route("Search/{keyword}/{currentPage:int?}/{pageSize:int?}")]
	public async Task<IActionResult> Search(string keyword, DateTime? birthDateFrom = null, DateTime? birthDateTo = null, int currentPage = 1, int pageSize = 100) =>
		this.GetQueryPolicy().Execute(() =>
		{
			if (!(birthDateFrom.HasValue && birthDateTo.HasValue || !birthDateFrom.HasValue && !birthDateTo.HasValue))
				return new BadRequestObjectResult(_errorLocalizer.GetLocalized(ErrorResources.DateRangeNotSelected));

			var result = _mediator.Send(new SearchQuery(
				new RequestSearchModel(keyword, birthDateFrom, birthDateTo, currentPage, pageSize))).Result;

			return Ok(result);
		});

	#endregion
}
