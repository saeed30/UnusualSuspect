using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnusualSuspect.DataLayer.Context;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.Entities;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Common;

/// <summary>
/// Source: My reference app https://github.com/dotnet-architecture/eShopOnWeb
/// Check it out if you need filtering/paging/etc.
/// Also consider Ardalis.Specification and its built-in generic repository
/// </summary>
public class EfRepository<T> : IAsyncRepository<T> where T : BaseEntity, new()
{
	private readonly IUnitOfWork _uow;
	private readonly ILogger<EfRepository<T>> logger;
	private readonly DbSet<T> _Entity;

	public EfRepository(IUnitOfWork uow, ILogger<EfRepository<T>> logger)
	{
		_uow = uow;
		this.logger = logger;
		_Entity = uow.Set<T>();

	}

	public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
	{
		return await _Entity.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
	}

	public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default)
	{
		return await _Entity.ToListAsync(cancellationToken);
	}

	/// <inheritdoc />
	public async Task<IReadOnlyList<T>> ListAllAsync(
		int perPage,
		int page,
					CancellationToken cancellationToken = default)
	{
		return await _Entity.Skip(perPage * (page - 1)).Take(perPage).ToListAsync(cancellationToken);
	}

	public T Add(T entity)
	{
		_Entity.Add(entity);
		return entity;
	}

	public void Update(T entity)
	{
		_uow.Entry(entity).State = EntityState.Modified;
	}

	public void Delete(T entity)
	{
		_Entity.Remove(entity);
	}

	public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
	{
		return await _uow.SaveChangesAsync(cancellationToken);
	}

	public void DeleteById(int id)
	{
		var entity = new T
		{
			Id = id
		};
		Delete(entity);
	}

	public async Task<int> ExecuteDeleteByIdAsync(int id, CancellationToken cancellationToken = default)
	{
		return await _Entity.Where(x => x.Id == id).ExecuteDeleteAsync(cancellationToken);
	}
}
