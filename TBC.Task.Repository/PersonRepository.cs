using Microsoft.EntityFrameworkCore;
using TBC.Task.Domain;
using TBC.Task.Domain.Interfaces.Repositories;
using TBC.Task.Repository.Database;

namespace TBC.Task.Repository;

public sealed class PersonRepository : RepositoryBase<Person>, IPersonRepository
{
	public PersonRepository(PersonsDbContext context) : base(context) { }

	public Person GetIncludeCity(int id) => _dbSet
		.Where(x => x.Id == id)
		.Include(x => x.City)
		.First();

	public IQueryable<Person> QuickSearch(string keyword, int currentPage, int pageSize) => _dbSet
		.Where(x =>
			x.FirstName.Contains(keyword) ||
			x.LastName.Contains(keyword) ||
			x.PersonalNumber.Contains(keyword))
		.Skip((currentPage - 1) * pageSize)
		.Take(pageSize);

	public IQueryable<Person> Search(string keyword, int currentPage, int pageSize) => _dbSet
		.Include(x => x.City)
		.Where(x =>
			x.FirstName.Contains(keyword) ||
			x.LastName.Contains(keyword) ||
			x.PersonalNumber.Contains(keyword) ||
			x.BirthDate.ToString("dd.MM.yyy").Contains(keyword) ||
			(x.MobilePhone ?? string.Empty).Contains(keyword) ||
			(x.HomePhone ?? string.Empty).Contains(keyword) ||
			(x.WorkPhone ?? string.Empty).Contains(keyword) ||
			(x.City.Name ?? string.Empty).Contains(keyword)
		)
		.Skip((currentPage - 1) * pageSize)
		.Take(pageSize);
}
