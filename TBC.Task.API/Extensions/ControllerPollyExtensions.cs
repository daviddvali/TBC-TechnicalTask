using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Wrap;
using TBC.Task.API.Controllers;
using TBC.Task.API.Localization;
using TBC.Task.API.Resources;

namespace TBC.Task.API.Extensions;

internal static class ControllerPollyExtensions
{
	public static PolicyWrap<IActionResult> GetPersonPolicy(this PersonsController controller) => Policy.Wrap(
		Policy<IActionResult>
			.Handle<TaskCanceledException>()
			.Fallback(_ => controller.NoContent()),
		Policy<IActionResult>
			.Handle<AggregateException>(_ => _.InnerException is KeyNotFoundException)
			.Fallback(_ => controller.NotFound(controller.ErrorLocalizer.GetLocalized(ErrorResources.PersonNotFound))));

	public static PolicyWrap<IActionResult> GetRelatedPersonPolicy(this PersonsController controller) => Policy.Wrap(
		Policy<IActionResult>
			.Handle<TaskCanceledException>()
			.Fallback(_ => controller.NoContent()),
		Policy<IActionResult>
			.Handle<AggregateException>(_ => _.InnerException is KeyNotFoundException)
			.Fallback(_ => controller.NotFound(controller.ErrorLocalizer.GetLocalized(ErrorResources.RelationNotFound))));

	public static Policy<IActionResult> GetQueryPolicy(this PersonsController controller) =>
		Policy<IActionResult>
			.Handle<TaskCanceledException>()
			.Fallback(_ => controller.NoContent());
}