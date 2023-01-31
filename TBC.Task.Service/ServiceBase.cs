﻿using System.Linq.Expressions;
using TBC.Task.Domain.Interfaces.Repositories;
using TBC.Task.Domain.Interfaces.Services;

namespace TBC.Task.Service;

public abstract class ServiceBase<TEntity, TRepository> : IService<TEntity>
	where TEntity : class
	where TRepository : IRepository<TEntity>
{
	private readonly TRepository _repository;

	protected ServiceBase(TRepository repository) =>
		_repository = repository;

	public virtual TEntity Get(params object[] id) =>
		_repository.Get(id);

	public IEnumerable<TEntity> Set(Expression<Func<TEntity, bool>> predicate) =>
		_repository.Set(predicate);

	public IEnumerable<TEntity> Set() =>
		_repository.Set();

	public virtual void Insert(TEntity entity)
	{
		_repository.Insert(entity);
		_repository.SaveChanges();
	}

	public virtual void Update(TEntity entity)
	{
		_repository.Update(entity);
		_repository.SaveChanges();
	}

	public virtual void Delete(object id) =>
		Delete(Get(id));

	public virtual void Delete(TEntity entity)
	{
		_repository.Delete(entity);
		_repository.SaveChanges();
	}
}