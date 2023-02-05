using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using TBC.Task.API.Controllers;
using TBC.Task.API.Extensions;
using TBC.Task.API.Localization;
using TBC.Task.API.Models;
using TBC.Task.API.Resources;

namespace TBC.Task.API.ActionFilters;

internal abstract class RequestDataValidationBaseAttribute : ActionFilterAttribute
{
	protected readonly IStringLocalizer<ErrorResources> _localizer;

	public RequestDataValidationBaseAttribute(IStringLocalizer<ErrorResources> localizer) =>
		_localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
}
