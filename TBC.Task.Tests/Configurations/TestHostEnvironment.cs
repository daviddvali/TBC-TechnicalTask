using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace TBC.Task.Tests.Configurations;

public class TestHostEnvironment : IHostEnvironment
{
    public string EnvironmentName { get; set; } = "TBC API Tests";
    public string ApplicationName { get; set; } = "TBC.Task.Tests";
    public string ContentRootPath { get; set; } = Directory.GetCurrentDirectory();
    public IFileProvider ContentRootFileProvider { get; set; } = new PhysicalFileProvider(Directory.GetCurrentDirectory());
}
