namespace TBC.Task.Service.Interfaces.Services;

public interface ICommandService<in TEntity>
{
    int? Insert(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}
