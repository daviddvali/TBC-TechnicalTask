namespace TBC.Task.API.Localization;

internal static class ConfigurationExtensions
{
    public static void ConfigureLocalization(this WebApplication app)
    {
        var supportedCultures = new[] { "en-US", "ka-Ge" };
        var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);

        app.UseRequestLocalization(localizationOptions);
    }
}