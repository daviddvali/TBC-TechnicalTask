using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using TBC.Task.API.Extensions;
using TBC.Task.API.Localization;
using TBC.Task.API.Models;
using TBC.Task.API.Resources;

namespace TBC.Task.API.ActionFilters;

internal sealed class DataValidationAttribute : ActionFilterAttribute
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
			if (!person.IsValid())
			{
				context.Result = new BadRequestObjectResult(
					string.Join(Environment.NewLine, GetValidationErrorDetails(person)));
			}
		}
	}

	private IEnumerable<string> GetValidationErrorDetails(RequestPersonModel person)
	{
		if (!person.IsFirstNameValid())
			yield return _localizer.GetLocalized(ErrorResources.FirstNameNotValid);
		if (!person.IsLastNameValid())
			yield return _localizer.GetLocalized(ErrorResources.LastNameNotValid);
		if (!person.IsPersonalNumberValid())
			yield return _localizer.GetLocalized(ErrorResources.PersonalNumberNotValid);
		if (!person.IsBirthDateValid())
			yield return _localizer.GetLocalized(ErrorResources.BirthDateNotValid);
		if (!person.IsMobilePhoneValid())
			yield return _localizer.GetLocalized(ErrorResources.MobilePhoneNotValid);
		if (!person.IsHomePhoneValid())
			yield return _localizer.GetLocalized(ErrorResources.HomePhoneNotValid);
		if (!person.IsWorkPhoneValid())
			yield return _localizer.GetLocalized(ErrorResources.WorkPhoneNotValid);
	}
}
