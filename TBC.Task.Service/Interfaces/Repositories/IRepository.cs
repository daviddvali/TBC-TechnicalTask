using System.Linq.Expressions;

namespace TBC.Task.Service.Interfaces.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
	TEntity Get(params object[] id);
	IQueryable<TEntity> Set(Expression<Func<TEntity, bool>> predicate);
	IQueryable<TEntity> Set();

	void Insert(TEntity entity);
	void Update(TEntity entity);
	void InsertOrUpdate(TEntity entity);
	void Delete(TEntity entity);
	void Delete(object id);

	int SaveChanges();
	Task<int> SaveChangesAsync();
}
