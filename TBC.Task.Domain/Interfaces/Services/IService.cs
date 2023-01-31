using System.Linq.Expressions;

namespace TBC.Task.Domain.Interfaces.Services;

public interface IService<TEntity>
{
	TEntity Get(params object[] id);
	IEnumerable<TEntity> Set(Expression<Func<TEntity, bool>> predicate);
	IEnumerable<TEntity> Set();

	int? Insert(TEntity entity);
	void Update(TEntity entity);
	void Delete(object id);
	void Delete(TEntity entity);
}
