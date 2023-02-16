using Microsoft.AspNetCore.Mvc;

namespace TBC.Task.Tests.Extensions;

internal static class ActionResultExtensions
{
    public static T GetPropertyFromOkObjectResult<T>(this OkObjectResult response, string propertyName)
    {
        var result = response.Value!;
        var value = result
            .GetType()!
            .GetProperty(propertyName)!
            .GetValue(result);

        return (T) Convert.ChangeType(value, typeof(T))!;
    }

    public static OkObjectResult? ToOkObjectResult(this IActionResult response) =>
        response as OkObjectResult;

    public static FileContentResult? ToFileContentResult(this IActionResult response) =>
        response as FileContentResult;
}
