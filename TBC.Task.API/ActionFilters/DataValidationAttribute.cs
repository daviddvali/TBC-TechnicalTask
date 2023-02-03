using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using TBC.Task.API.Models;
using Microsoft.Extensions.Localization;
using TBC.Task.API.Localization;
using TBC.Task.API.Resources;

namespace TBC.Task.API.ActionFilters;

internal class DataValidationAttribute : ActionFilterAttribute
{
	private readonly IStringLocalizer<ErrorResources> _localizer;

	public DataValidationAttribute(IStringLocalizer<ErrorResources> localizer) =>
		_localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));

	public override void OnActionExecuting(ActionExecutingContext context)
	{
		base.OnActionExecuting(context);

		if (context.ActionArguments.ContainsKey("model") &&
			context.ActionArguments["model"] is RequestPersonModel)
		{
			var person = (RequestPersonModel) context.ActionArguments["model"]!;
			if (person.FirstName != null)
			{
				context.Result = new BadRequestObjectResult(_localizer.GetLocalized(ErrorResources.FirstNameNotValid));
			}
		}
		
	}
}
