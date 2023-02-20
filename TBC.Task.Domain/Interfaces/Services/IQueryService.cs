using System.Linq.Expressions;

namespace TBC.Task.Domain.Interfaces.Services;

public interface IQueryService<TEntity>
{
    TEntity Get(params object[] id);
    IEnumerable<TEntity> Set(Expression<Func<TEntity, bool>> predicate);
    IEnumerable<TEntity> Set();
}
