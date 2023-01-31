namespace TBC.Task.API.StartupConfigurations;

public static class SetupApplicationConfiguration
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
