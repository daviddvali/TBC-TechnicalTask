using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using TBC.Task.API.Controllers;
using TBC.Task.API.Models;
using TBC.Task.Tests.Bases;
using TBC.Task.Tests.Extensions;
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

    [Theory]
    [InlineData(10)]
    [InlineData(11)]
    [InlineData(12)]
    [InlineData(13)]
    [InlineData(14)]
    public async SystemTasks.Task ShouldDeleteById(int id)
    {
        var deleteResponse = await _controller.Delete(id)!;
        Assert.IsType<NoContentResult>(deleteResponse);

        var getResponse = await _controller.Get(id)!;
        Assert.IsType<NotFoundObjectResult>(getResponse);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(1, 3)]
    [InlineData(1, 4)]
    [InlineData(1, 5)]
    [InlineData(1, 6)]
    public async SystemTasks.Task ShouldAddRelatedPerson(int fromId, int toId)
    {
        var addRelatedPersonResponse = await _controller.AddRelatedPerson(fromId, toId)!;
        Assert.IsType<OkResult>(addRelatedPersonResponse);

        var getPersonResponse = await _controller.Get(fromId)!;
        var person = (ResponsePersonWithRelatedModel) (getPersonResponse as OkObjectResult)!.Value!;
        Assert.Contains(toId, person.RelatedTo!.Select(x => x.Id));
    }

    [Theory]
    [InlineData(2, 3)]
    [InlineData(2, 4)]
    [InlineData(2, 5)]
    [InlineData(2, 6)]
    [InlineData(2, 7)]
    public async SystemTasks.Task ShouldGetRelatedPersonsCount(int fromId, int toId)
    {
        var addRelatedPersonResponse = await _controller.AddRelatedPerson(fromId, toId)!;
        Assert.IsType<OkResult>(addRelatedPersonResponse);

        var getRelatedPersonsCountResponse = await _controller.GetRelatedPersonsCount(fromId)!;
        Assert.IsType<OkObjectResult>(getRelatedPersonsCountResponse);

        int relatedPersonsCount = (getRelatedPersonsCountResponse as OkObjectResult)!
            .GetPropertyFromOkObjectResult<int>("relatedPersonsCount");

        var getPersonResponse = await _controller.Get(fromId)!;
        var person = (ResponsePersonWithRelatedModel) (getPersonResponse as OkObjectResult)!.Value!;
        Assert.Equal(relatedPersonsCount, person.RelatedTo!.Count());
    }

    [Theory]
    [InlineData(3, 4)]
    [InlineData(3, 5)]
    [InlineData(3, 6)]
    [InlineData(3, 7)]
    [InlineData(3, 8)]
    public async SystemTasks.Task ShouldDeleteRelatedPerson(int fromId, int toId)
    {


        var addRelatedPersonResponse = await _controller.AddRelatedPerson(fromId, toId)!;
        Assert.IsType<OkResult>(addRelatedPersonResponse);

        var deleteRelatedPersonResponse = await _controller.DeleteRelatedPerson(fromId, toId)!;
        Assert.IsType<NoContentResult>(deleteRelatedPersonResponse);

        var getPersonResponse = await _controller.Get(fromId)!;
        var person = (ResponsePersonWithRelatedModel) (getPersonResponse as OkObjectResult)!.Value!;
        Assert.DoesNotContain(toId, person.RelatedTo!.Select(x => x.Id));
    }

    [Fact]
    public async SystemTasks.Task ShouldAddPersons()
    {
        // Insert data and collect ids.
        var ids = new List<int>();
        foreach (var person in GetTestDataFromJson<RequestPersonModel>("appvalidtestdata.json"))
        {
            var response = await _controller.Create(person);
            Assert.IsType<OkObjectResult>(response);

            ids.Add((response as OkObjectResult)!.GetPropertyFromOkObjectResult<int>("id"));
        }

        // Check id data was inserted successfully.
        foreach (var id in ids)
        {
            var response = await _controller.Get(id)!;
            Assert.IsType<OkObjectResult>(response);
        }
    }

    [Fact]
    public async SystemTasks.Task ShouldNotAddPersons()
    {
        foreach (var person in GetTestDataFromJson<RequestPersonModel>("appinvalidtestdata.json"))
        {
            var response = await _controller.Create(person);
            Assert.IsType<OkObjectResult>(response);
        }
    }
}