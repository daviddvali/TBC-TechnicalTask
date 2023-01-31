using TBC.Task.API.StartupConfigurations;

var app = WebApplication
	.CreateBuilder(args)
	.RegisterServices()
	.Build();

app.SetupApplication().Run();