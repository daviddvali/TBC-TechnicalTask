using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using TBC.Task.API.Controllers;
using TBC.Task.API.Localization;
using TBC.Task.API.Models;
using TBC.Task.API.Resources;

namespace TBC.Task.API.ActionFilters;

internal sealed class SearchRequestDataValidationAttribute : RequestDataValidationBaseAttribute
{
	public SearchRequestDataValidationAttribute(IStringLocalizer<ErrorResources> localizer) : base(localizer) { }

	public override void OnActionExecuting(ActionExecutingContext context)
	{
		base.OnActionExecuting(context);

		var actionName = (context.ActionDescriptor as ControllerActionDescriptor)?.ActionName;

		if (actionName is nameof(PersonsController.QuickSearch) or nameof(PersonsController.Search) &&
			context.ActionArguments.ContainsKey("model") &&
			context.ActionArguments["model"] is RequestPersonModel)
		{
			var keyword = context.ActionArguments["keyword"]?.ToString();
			var currentPage = (int) context.ActionArguments["currentPage"]!;
			var pageSize = (int) context.ActionArguments["pageSize"]!;

			if (string.IsNullOrEmpty(keyword) || currentPage < 1 || pageSize < 1)
			{
				context.Result = new BadRequestObjectResult(
					_localizer.GetLocalized(ErrorResources.SearchParametersNotValid));
			}
		}
	}
}
