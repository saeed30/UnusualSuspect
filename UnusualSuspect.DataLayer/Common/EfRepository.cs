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
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Common;

public class EfRepository<T> : EfRepository<T, int> where T : BaseEntity, new()
{
	public EfRepository(IUnitOfWork uow, ILogger<EfRepository<T, int>> logger) : base(uow, logger)
	{
	}
}
/// <summary>
/// Source: My reference app https://github.com/dotnet-architecture/eShopOnWeb
/// Check it out if you need filtering/paging/etc.
/// Also consider Ardalis.Specification and its built-in generic repository
/// </summary>
public class EfRepository<T, TY> : IAsyncRepository<T, TY> where T : BaseEntity<TY>, new() where TY : IEquatable<TY>
{
	private readonly IUnitOfWork uow;
	private readonly ILogger<EfRepository<T, TY>> logger;
	private readonly DbSet<T> entity;

	public EfRepository(IUnitOfWork uow, ILogger<EfRepository<T, TY>> logger)
	{
		this.uow = uow;
		this.logger = logger;
		entity = uow.Set<T>();

	}

	public virtual async Task<T?> GetByIdAsync(TY id, CancellationToken cancellationToken = default)
	{
		return await entity.FirstOrDefaultAsync(a => a.Id.Equals(id), cancellationToken);
	}

	public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default)
	{
		return await entity.ToListAsync(cancellationToken);
	}

	/// <inheritdoc />
	public async Task<IReadOnlyList<T>> ListAllAsync(
		int perPage,
		int page,
					CancellationToken cancellationToken = default)
	{
		return await entity.Skip(perPage * (page - 1)).Take(perPage).ToListAsync(cancellationToken);
	}

	public T Add(T entity)
	{
		this.entity.Add(entity);
		return entity;
	}

	public void Update(T entity)
	{
		uow.Entry(entity).State = EntityState.Modified;
	}

	public void Delete(T entity)
	{
		this.entity.Remove(entity);
	}

	public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
	{
		return await uow.SaveChangesAsync(cancellationToken);
	}

	public void DeleteById(TY id)
	{
		var entity = new T
		{
			Id = id
		};
		Delete(entity);
	}

	public async Task<int> ExecuteDeleteByIdAsync(TY id, CancellationToken cancellationToken = default)
	{
		return await entity.Where(x => x.Id.Equals(id)).ExecuteDeleteAsync(cancellationToken);
	}
}
