using System.Globalization;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TBC.Task.Domain;
using TBC.Task.Domain.Interfaces.Repositories;
using TBC.Task.Repository.Database;

namespace TBC.Task.Repository;

public sealed class PersonRepository : RepositoryBase<Person>, IPersonRepository
{
	public PersonRepository(PersonsDbContext context) : base(context) { }

	public Person? GetIncludeCity(int id) => _dbSet
		.Where(x => x.Id == id)
		.Include(x => x.City)
		.FirstOrDefault();

	public (IQueryable<Person>, int) QuickSearch(string keyword, int currentPage, int pageSize)
	{
		Expression<Func<Person, bool>> predicate = x =>
			x.FirstName.Contains(keyword) ||
			x.LastName.Contains(keyword) ||
			x.PersonalNumber.Contains(keyword);

		var result = _dbSet
			.Where(predicate)
			.Skip((currentPage - 1) * pageSize)
			.Take(pageSize);

		var resultTotalCount = _dbSet
			.Where(predicate)
			.Count();

		return (result, resultTotalCount);
	}

	public (IQueryable<Person>, int) Search(string keyword, DateTime? birthDateFrom, DateTime? birthDateTo, int currentPage, int pageSize)
	{
		Expression<Func<Person, bool>> predicate = x =>
			x.FirstName.Contains(keyword) ||
			x.LastName.Contains(keyword) ||
			x.PersonalNumber.Contains(keyword) ||
			((birthDateFrom.HasValue && x.BirthDate >= birthDateFrom) && (birthDateTo.HasValue && x.BirthDate <= birthDateTo)) ||
			(x.MobilePhone != null && x.MobilePhone.Contains(keyword)) ||
			(x.HomePhone != null && x.HomePhone.Contains(keyword)) ||
			(x.WorkPhone != null && x.WorkPhone.Contains(keyword)) ||
			(x.City != null && x.City.Name.Contains(keyword));
		
		var result = _dbSet
			.Include(x => x.City)
			.Where(predicate)
			.Skip((currentPage - 1) * pageSize)
			.Take(pageSize);

		var resultTotalCount = _dbSet
			.Include(x => x.City)
			.Where(predicate)
			.Count();

		return (result, resultTotalCount);
	}
}
