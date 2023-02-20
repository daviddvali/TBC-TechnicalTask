using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TBC.Task.Service.Interfaces.Repositories;
using TBC.Task.Repository.Database;

namespace TBC.Task.Repository;

public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
{
	private readonly PersonsDbContext _context;
	protected readonly DbSet<TEntity> _dbSet;

	protected RepositoryBase(PersonsDbContext context)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
		_dbSet = context.Set<TEntity>();
	}

	public virtual TEntity Get(params object[] id) =>
		_dbSet.Find(id) ?? throw new KeyNotFoundException($"Record with key {id} not found");

	public virtual IQueryable<TEntity> Set(Expression<Func<TEntity, bool>> predicate) =>
		_context.Set<TEntity>().Where(predicate);

	public virtual IQueryable<TEntity> Set() =>
		_context.Set<TEntity>();

	public virtual void Insert(TEntity entity) =>
		_dbSet.Add(entity);

	public virtual void Update(TEntity entity)
	{
		_dbSet.Attach(entity);
		_context.Entry(entity).State = EntityState.Modified;
	}

	public virtual void InsertOrUpdate(TEntity entity)
	{
		var entry = _context.Entry(entity);
		if (entry == null || entry.State == EntityState.Detached)
		{
			Insert(entity);
		}
		else
		{
			Update(entity);
		}
	}

	public virtual void Delete(object id) =>
		Delete(Get(id));

	public virtual void Delete(TEntity entity)
	{
		if (_context.Entry(entity).State == EntityState.Detached)
		{
			_dbSet.Attach(entity);
		}
		_dbSet.Remove(entity);
	}

	public int SaveChanges() =>
		_context.SaveChanges();

	public Task<int> SaveChangesAsync() =>
		_context.SaveChangesAsync();
}
