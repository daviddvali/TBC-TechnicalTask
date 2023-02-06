using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using ILogger = Serilog.ILogger;

namespace TBC.Task.API.ExceptionHandler;

internal static class ExceptionMiddlewareExtensions
{
	public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
	{
		app.UseExceptionHandler(appError =>
		{
			appError.Run(async context =>
			{
				context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
				context.Response.ContentType = "application/json";

				var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
				if (contextFeature != null)
				{
					logger.Error(contextFeature.Error, "Application Error");
					await context.Response.WriteAsync(
						new ErrorDetails(context.Response.StatusCode, "Internal Server Error."
					).ToString());
				}
			});
		});
	}
}
