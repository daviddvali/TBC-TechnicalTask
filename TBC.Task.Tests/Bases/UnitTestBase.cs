using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TBC.Task.Tests.Configurations;

namespace TBC.Task.Tests.Bases;

public abstract class UnitTestBase
{
    protected readonly ServiceCollection _services;

    protected UnitTestBase()
    {
        _services = new ServiceCollection();
        var environment = new TestHostEnvironment();
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        _services
            .ConfigureServices(environment, configuration)
            .ConfigureDbContext();
    }

    protected static List<T> GetTestDataFromJson<T>(string filePath) =>
        JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(filePath));
}
