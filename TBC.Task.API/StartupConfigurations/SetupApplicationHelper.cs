namespace TBC.Task.API.StartupConfigurations;

internal static class SetupApplicationHelper
{
	public static WebApplication SetupApplication(this WebApplication app)
	{
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
			app.UseDeveloperExceptionPage();
		}

		app.UseHttpsRedirection();
		app.UseAuthorization();
		app.MapControllers();

		return app;
	}
}
