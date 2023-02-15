using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using TBC.Task.API.Controllers;
using TBC.Task.API.Models;
using TBC.Task.Tests.Bases;
using SystemTasks = System.Threading.Tasks;

namespace TBC.Task.Tests;

[Collection("DbFixture")]
public sealed class PersonsUnitTests : UnitTestBase
{
    private readonly PersonsController _controller;

    public PersonsUnitTests()
    {
        _controller = _services
            .BuildServiceProvider()
            .GetService<PersonsController>()!;
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async SystemTasks.Task ShouldGetById(int id)
    {
        var response = await _controller.Get(id)!;
        Assert.IsType<OkObjectResult>(response);

        //var person = (ResponsePersonWithRelatedModel) result.Value!;
    }
    
    [Theory]
    [InlineData(1001)]
    [InlineData(1002)]
    [InlineData(1003)]
    [InlineData(1004)]
    [InlineData(1005)]
    public async SystemTasks.Task ShouldNotGetById(int id)
    {
        var response = await _controller.Get(id)!;
        Assert.IsType<NotFoundObjectResult>(response);
    }
}