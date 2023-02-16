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
        var person = (ResponsePersonWithRelatedModel) getPersonResponse.ToOkObjectResult()!.Value!;
        Assert.Contains(toId, person.RelatedTo!.Select(x => x.Id));
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
        var person = (ResponsePersonWithRelatedModel) getPersonResponse.ToOkObjectResult()!.Value!;
        Assert.DoesNotContain(toId, person.RelatedTo!.Select(x => x.Id));
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

        var relatedPersonsCount = getRelatedPersonsCountResponse.ToOkObjectResult()!
            .GetPropertyFromOkObjectResult<int>("relatedPersonsCount");

        var getPersonResponse = await _controller.Get(fromId)!;
        var person = (ResponsePersonWithRelatedModel) getPersonResponse.ToOkObjectResult()!.Value!;
        Assert.Equal(relatedPersonsCount, person.RelatedTo!.Count());
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

            ids.Add(response.ToOkObjectResult()!.GetPropertyFromOkObjectResult<int>("id"));
        }

        // Check id data was inserted successfully.
        foreach (var id in ids)
        {
            var response = await _controller.Get(id)!;
            Assert.IsType<OkObjectResult>(response);
        }
    }

    [InlineData(41)]
    [InlineData(42)]
    [InlineData(43)]
    [InlineData(44)]
    [InlineData(45)]
    [InlineData(51)]
    [InlineData(52)]
    [InlineData(53)]
    [InlineData(54)]
    [InlineData(55)]
    public async SystemTasks.Task ShouldUpdatePersons(int id)
    {
        var getPersonResponse = await _controller.Get(id)!;
        var person = (ResponsePersonModel) getPersonResponse.ToOkObjectResult()!.Value!;

        RequestPersonModel model = new()
        {
            FirstName = person.FirstName,
            LastName = person.LastName,
            PersonalNumber = person.PersonalNumber,
            Gender = person.Gender,
            BirthDate = person.BirthDate,
            MobilePhone = person.MobilePhone,
            WorkPhone = person.WorkPhone,
            HomePhone = person.HomePhone,
            CityId = person.CityId
        };

        var updatePersonResponse = await _controller.Update(id, model)!;
        Assert.IsType<NoContentResult>(updatePersonResponse);
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

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async SystemTasks.Task ShouldUploadPhotoById(int id)
    {
        var photoSize = Random.Shared.Next(1, 5) * 1024 * 1024;
        var file = GetMockFile("photo.jpg", photoSize);

        var response = await _controller.UploadPhoto(id, file)!;
        Assert.IsType<OkObjectResult>(response);

        var responseId = response.ToOkObjectResult()!.GetPropertyFromOkObjectResult<int>("id");
        var responsePhotoSize = response.ToOkObjectResult()!.GetPropertyFromOkObjectResult<int>("photoSize");
        Assert.True(id == responseId && photoSize == responsePhotoSize);
    }

    [Theory]
    [InlineData(1, false)]
    [InlineData(1002, false)]
    [InlineData(1003, false)]
    [InlineData(1004, true)]
    [InlineData(1005, true)]
    public async SystemTasks.Task ShouldNotUploadPhotoById(int id, bool sendPhotoData)
    {
        var file = GetMockFile("photo.jpg", sendPhotoData ? GetRandomPhotoSize() : 0);
        var response = await _controller.UploadPhoto(id, file)!;
        Assert.IsNotType<OkObjectResult>(response);
    }

    [Theory]
    [InlineData(21)]
    [InlineData(22)]
    [InlineData(23)]
    [InlineData(24)]
    [InlineData(25)]
    public async SystemTasks.Task ShouldGetPhotoById(int id)
    {
        var photoSize = GetRandomPhotoSize();
        var file = GetMockFile("photo.jpg", photoSize);

        var uploadPhotoResponse = await _controller.UploadPhoto(id, file)!;
        Assert.IsType<OkObjectResult>(uploadPhotoResponse);

        var getPhotoResponse = await _controller.GetPhoto(id)!;
        Assert.IsType<FileContentResult>(getPhotoResponse);

        var responseId = uploadPhotoResponse.ToOkObjectResult()!.GetPropertyFromOkObjectResult<int>("id");
        var responsePhotoSize = getPhotoResponse.ToFileContentResult()!.FileContents.Length;
        Assert.True(id == responseId && photoSize == responsePhotoSize);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(31)]
    [InlineData(32)]
    [InlineData(33)]
    [InlineData(34)]
    [InlineData(35)]
    public async SystemTasks.Task ShouldFindByQuickSearch(int id)
    {
        static bool HasSearchedData(ResponsePersonModel person, string keyword) =>
            person.FirstName.Contains(keyword) ||
            person.LastName.Contains(keyword) ||
            person.PersonalNumber.Contains(keyword);

        async SystemTasks.Task TestSearch(PersonsController controller, string keyword, ResponsePersonModel expected)
        {
            var searchResponse = await controller.QuickSearch(keyword)!;
            var searchResult = (ResponseSearchModel) searchResponse.ToOkObjectResult()!.Value!;
            Assert.NotEmpty(searchResult.Result);
            searchResult.Result.ToList().ForEach(p =>
                Assert.True(HasSearchedData(p, keyword)));
        }

        var getResponse = await _controller.Get(id)!;
        var person = (ResponsePersonModel) getResponse.ToOkObjectResult()!.Value!;

        await TestSearch(_controller, person.FirstName, person);
        await TestSearch(_controller, person.FirstName[..^1], person);
        await TestSearch(_controller, person.FirstName[1..], person);

        await TestSearch(_controller, person.LastName, person);
        await TestSearch(_controller, person.LastName[..^1], person);
        await TestSearch(_controller, person.LastName[1..], person);

        await TestSearch(_controller, person.PersonalNumber, person);
        await TestSearch(_controller, person.PersonalNumber[..^1], person);
        await TestSearch(_controller, person.PersonalNumber[1..], person);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(31)]
    [InlineData(32)]
    [InlineData(33)]
    [InlineData(34)]
    [InlineData(35)]
    public async SystemTasks.Task ShouldNotFindByQuickSearch(int id)
    {
        async SystemTasks.Task TestSearch(PersonsController controller, string keyword)
        {
            var searchResponse = await controller.QuickSearch(keyword)!;
            var searchResult = (ResponseSearchModel) searchResponse.ToOkObjectResult()!.Value!;
            Assert.Empty(searchResult.Result);
        }

        var getResponse = await _controller.Get(id)!;
        var person = (ResponsePersonModel) getResponse.ToOkObjectResult()!.Value!;

        await TestSearch(_controller, $"a{person.FirstName}b");
        await TestSearch(_controller, $"a{person.LastName}b");
        await TestSearch(_controller, $"a{person.PersonalNumber}b");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(31)]
    [InlineData(32)]
    [InlineData(33)]
    [InlineData(34)]
    [InlineData(35)]
    public async SystemTasks.Task ShouldFindBySearch(int id)
    {
        static bool HasSearchedData(
            ResponsePersonModel person,
            string keyword,
            DateTime? birthDateFrom = null,
            DateTime? birthDateTo = null)
        {
            return
                person.FirstName.Contains(keyword) ||
                person.LastName.Contains(keyword) ||
                person.PersonalNumber.Contains(keyword) ||
                person.MobilePhone != null && person.MobilePhone.Contains(keyword) ||
                person.HomePhone != null && person.HomePhone.Contains(keyword) ||
                person.WorkPhone != null && person.WorkPhone.Contains(keyword) ||
                person.City != null && person.City.Contains(keyword) ||
                ((birthDateFrom.HasValue && person.BirthDate >= birthDateFrom) &&
                 (birthDateTo.HasValue && person.BirthDate <= birthDateTo));
        }

        async SystemTasks.Task TestSearch(
            PersonsController controller,
            string keyword,
            ResponsePersonModel expected,
            DateTime? birthDateFrom = null,
            DateTime? birthDateTo = null)
        {
            var searchResponse = await controller.Search(keyword, birthDateFrom, birthDateTo)!;
            var searchResult = (ResponseSearchModel) searchResponse.ToOkObjectResult()!.Value!;
            Assert.NotEmpty(searchResult.Result);
            searchResult.Result.ToList().ForEach(p =>
                Assert.True(HasSearchedData(p, keyword, birthDateFrom, birthDateTo)));
        }

        var getResponse = await _controller.Get(id)!;
        var person = (ResponsePersonModel) getResponse.ToOkObjectResult()!.Value!;

        await TestSearch(_controller, person.FirstName, person);
        await TestSearch(_controller, person.FirstName[..^1], person);
        await TestSearch(_controller, person.FirstName[1..], person);
        await TestSearch(_controller, $"a{person.FirstName}b", person, person.BirthDate.AddDays(-1), person.BirthDate.AddDays(1));

        await TestSearch(_controller, person.LastName, person);
        await TestSearch(_controller, person.LastName[..^1], person);
        await TestSearch(_controller, person.LastName[1..], person);
        await TestSearch(_controller, $"a{person.LastName}b", person, person.BirthDate.AddDays(-1), person.BirthDate.AddDays(1));

        await TestSearch(_controller, person.PersonalNumber, person);
        await TestSearch(_controller, person.PersonalNumber[..^1], person);
        await TestSearch(_controller, person.PersonalNumber[1..], person);
        await TestSearch(_controller, $"a{person.PersonalNumber}b", person, person.BirthDate.AddDays(-1), person.BirthDate.AddDays(1));

        if (!string.IsNullOrEmpty(person.MobilePhone))
        {
            await TestSearch(_controller, person.MobilePhone, person);
            await TestSearch(_controller, person.MobilePhone[..^1], person);
            await TestSearch(_controller, person.MobilePhone[1..], person);
        }

        if (!string.IsNullOrEmpty(person.HomePhone))
        {
            await TestSearch(_controller, person.HomePhone, person);
            await TestSearch(_controller, person.HomePhone[..^1], person);
            await TestSearch(_controller, person.HomePhone[1..], person);
        }

        if (!string.IsNullOrEmpty(person.WorkPhone))
        {
            await TestSearch(_controller, person.WorkPhone, person);
            await TestSearch(_controller, person.WorkPhone[..^1], person);
            await TestSearch(_controller, person.WorkPhone[1..], person);
        }

        if (!string.IsNullOrEmpty(person.City))
        {
            await TestSearch(_controller, person.City, person);
            await TestSearch(_controller, person.City[..^1], person);
            await TestSearch(_controller, person.City[1..], person);
        }
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(31)]
    [InlineData(32)]
    [InlineData(33)]
    [InlineData(34)]
    [InlineData(35)]
    public async SystemTasks.Task ShouldNotFindBySearch(int id)
    {
        async SystemTasks.Task TestSearch(
            PersonsController controller,
            string keyword,
            DateTime? birthDateFrom = null,
            DateTime? birthDateTo = null)
        {
            var searchResponse = await controller.Search(keyword, birthDateFrom, birthDateTo)!;
            var searchResult = (ResponseSearchModel) searchResponse.ToOkObjectResult()!.Value!;
            Assert.Empty(searchResult.Result);
        }

        var getResponse = await _controller.Get(id)!;
        var person = (ResponsePersonModel) getResponse.ToOkObjectResult()!.Value!;

        await TestSearch(_controller, $"a{person.FirstName}b");
        await TestSearch(_controller, $"a{person.LastName}b");
        await TestSearch(_controller, $"a{person.PersonalNumber}b");

        if (!string.IsNullOrEmpty(person.MobilePhone))
        {
            await TestSearch(_controller, $"a{person.MobilePhone}b");
        }
        if (!string.IsNullOrEmpty(person.HomePhone))
        {
            await TestSearch(_controller, $"a{person.HomePhone}b");
        }
        if (!string.IsNullOrEmpty(person.WorkPhone))
        {
            await TestSearch(_controller, $"a{person.WorkPhone}b");
        }
        if (!string.IsNullOrEmpty(person.City))
        {
            await TestSearch(_controller, $"a{person.City}b");
        }
    }

    private static int GetRandomPhotoSize(int minMb = 1, int maxMb = 5) =>
        Random.Shared.Next(minMb, maxMb + 1) * 1024 * 1024;
}