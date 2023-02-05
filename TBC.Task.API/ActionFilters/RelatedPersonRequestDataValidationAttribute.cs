using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using TBC.Task.API.Controllers;
using TBC.Task.API.Localization;
using TBC.Task.API.Models;
using TBC.Task.API.Resources;

namespace TBC.Task.API.ActionFilters;

internal sealed class RelatedPersonRequestDataValidationAttribute : RequestDataValidationBaseAttribute
{
	public RelatedPersonRequestDataValidationAttribute(IStringLocalizer<ErrorResources> localizer) : base(localizer) { }

	public override void OnActionExecuting(ActionExecutingContext context)
	{
		base.OnActionExecuting(context);

		var actionName = (context.ActionDescriptor as ControllerActionDescriptor)?.ActionName;

		if (actionName is nameof(PersonsController.AddRelatedPerson) or nameof(PersonsController.DeleteRelatedPerson) &&
			context.ActionArguments.ContainsKey("model") &&
			context.ActionArguments["model"] is RequestPersonModel)
		{
			var from = (int) context.ActionArguments["from"]!;
			var to = (int) context.ActionArguments["to"]!;
			if (from.Equals(to))
			{
				context.Result = new BadRequestObjectResult(
					_localizer.GetLocalized(ErrorResources.FromAndToShouldDifferer));
			}
		}
	}
}
