using Serilog;
using TBC.Task.API.ExceptionHandler;
using ILogger = Serilog.ILogger;

namespace TBC.Task.API.StartupConfigurations;

internal static class SetupApplicationHelper
{
	public static WebApplication SetupApplication(this WebApplication app)
	{
		ILogger logger = Log.ForContext(typeof(SetupApplicationHelper));

		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
			app.UseDeveloperExceptionPage();
		}

		app.UseHttpsRedirection();
		app.UseAuthorization();
		app.MapControllers();
		app.ConfigureExceptionHandler(logger);

		return app;
	}
}
