using TBC.Task.API.Middlewares;
using TBC.Task.API.ApplicationConfigurations;

var app = WebApplication
	.CreateBuilder(args)
	.RegisterServices()
	.Build();

app.UseMiddleware<SetAcceptLanguageMiddleware>();
app.SetupApplication().Run();