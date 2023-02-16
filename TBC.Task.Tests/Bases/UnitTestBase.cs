using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
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

    protected static IFormFile GetMockFile(string fileName, int photoSize)
    {
        byte[] data = new byte[photoSize];
        Random.Shared.NextBytes(data);

        var file = new MemoryStream(data);
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.FileName).Returns(fileName);
        mockFile.Setup(f => f.Length).Returns(file.Length);
        mockFile.Setup(f => f.ContentType).Returns("image/jpg");
        mockFile.Setup(f => f.OpenReadStream()).Returns(file);

        return mockFile.Object;
    }

    protected static List<T> GetTestDataFromJson<T>(string filePath) =>
        JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(filePath));
}
