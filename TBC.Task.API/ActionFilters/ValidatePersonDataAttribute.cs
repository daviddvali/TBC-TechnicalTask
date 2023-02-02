using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using TBC.Task.API.Models;

namespace TBC.Task.API.ActionFilters;

public class ValidatePersonDataAttribute : ActionFilterAttribute
{
	public override void OnActionExecuting(ActionExecutingContext context)
	{
		var person = (RequestPersonModel)context.ActionArguments["model"]!;
		if (string.IsNullOrEmpty(person.FirstName))
		{
			context.Result = new BadRequestObjectResult("");
		}
		//if (person.FirstName != null)
		//{
		//	context.Result = new BadRequestObjectResult("One or more input parameters are null.");
		//}
	}
}
