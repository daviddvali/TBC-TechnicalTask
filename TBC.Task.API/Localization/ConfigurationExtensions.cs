using Microsoft.Extensions.Localization;
using TBC.Task.API.Resources;

namespace TBC.Task.API.Localization;

internal static class LocalizationExtensions
{
    public static string GetLocalized(this IStringLocalizer<ErrorResources> localizer, string key) =>
	    localizer[key].Value;
}
