using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TBC.Task.Domain;
using TBC.Task.Repository.Database;
using TBC.Task.Tests.Configurations;

namespace TBC.Task.Tests.Fixtures;

public sealed class DbFixture : IDisposable
{
    private readonly PersonsDbContext _dbContext;

    public DbFixture()
    {
        var services = new ServiceCollection();
        services.ConfigureDbContext();

        _dbContext = services
            .BuildServiceProvider()
            .GetService<PersonsDbContext>()!;

        InsertTestData();
    }

    public void Dispose() => _dbContext.Dispose();

    private void InsertTestData()
    {
        static List<Person> GetTestData()
        {
            var json = File.ReadAllText("apptestdata.json");
            var persons = JsonConvert.DeserializeObject<List<Person>>(json);

            return persons;
        }

        _dbContext.Cities.Add(new City { Name = "Tbilisi" });
        _dbContext.Cities.Add(new City { Name = "Batumi" });
        _dbContext.Cities.Add(new City { Name = "Kutaisi" });
        _dbContext.Cities.Add(new City { Name = "Rustavi" });
        _dbContext.Cities.Add(new City { Name = "Gori" });
        _dbContext.SaveChanges();
        
        GetTestData().ForEach(p => _dbContext.Persons.Add(p));
        _dbContext.SaveChanges();
    }
}

[CollectionDefinition("DbFixture")]
public sealed class PersonsCollection : ICollectionFixture<DbFixture> { }
