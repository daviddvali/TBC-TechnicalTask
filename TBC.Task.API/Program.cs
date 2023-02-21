using TBC.Task.API.ApplicationConfigurations;
using TBC.Task.API.Middlewares;

namespace TBC.Task.API;

public class Program
{
    private static void Main(string[] args)
    {
        var app = WebApplication
            .CreateBuilder(args)
            .RegisterServices()
            .Build();

        app.UseMiddleware<SetAcceptLanguageMiddleware>();
        app.SetupApplication().Run();
    }
}