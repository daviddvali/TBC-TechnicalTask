using Serilog;
using Serilog.Exceptions;

namespace TBC.Task.API.StartupConfigurations;

internal static class LoggerConfigureHelper
{
	public static void ConfigureLogger(this WebApplicationBuilder builder)
	{
		var logger = new LoggerConfiguration()
			.ReadFrom.Configuration(builder.Configuration)
			.Enrich.WithExceptionDetails()
			.CreateLogger();
		builder.Logging.ClearProviders();
		builder.Logging.AddSerilog(logger);
	}
}
