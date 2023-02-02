using Serilog;
using TBC.Task.API.ExceptionHandler;
using TBC.Task.API.Localization;
using ILogger = Serilog.ILogger;

namespace TBC.Task.API.ApplicationConfigurations;

internal static class ConfigurationApplicationExtensions
{
	public static WebApplication SetupApplication(this WebApplication app)
	{
		ILogger logger = Log.ForContext(typeof(ConfigurationApplicationExtensions));

		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
			app.UseDeveloperExceptionPage();
		}

		app.UseHttpsRedirection();
		app.UseAuthorization();
		app.MapControllers();
		app.ConfigureLocalization();
		app.ConfigureExceptionHandler(logger);

		return app;
	}
}
