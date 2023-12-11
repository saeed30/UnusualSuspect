using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.DataLayer.Common;

public class EfRepository<T>(IUnitOfWork uow, ILogger<EfRepository<T, int>> logger) : EfRepository<T, int>(uow, logger)
  where T : BaseEntity, new();
/// <summary>
/// Source: My reference app https://github.com/dotnet-architecture/eShopOnWeb
/// Check it out if you need filtering/paging/etc.
/// Also consider Ardalis.Specification and its built-in generic repository
/// </summary>
public class EfRepository<T, TY>(IUnitOfWork uow, ILogger<EfRepository<T, TY>> logger) : IAsyncRepository<T, TY>
  where T : BaseEntity<TY>, new()
  where TY : IEquatable<TY>
{
	private readonly DbSet<T> baseEntity = uow.Set<T>();

  public virtual async Task<T?> GetByIdAsync(TY id, CancellationToken cancellationToken = default)
	{
		return await baseEntity.FirstOrDefaultAsync(a => a.Id.Equals(id), cancellationToken);
	}

  public IQueryable<T> GetAll()
  {
    return baseEntity;
  }

  public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default)
	{
		return await baseEntity.ToListAsync(cancellationToken);
	}

	/// <inheritdoc />
	public async Task<IReadOnlyList<T>> ListAllAsync(
		int perPage,
		int page,
					CancellationToken cancellationToken = default)
	{
		return await baseEntity.Skip(perPage * (page - 1)).Take(perPage).ToListAsync(cancellationToken);
	}

	public T Add(T entity)
	{
    baseEntity.Add(entity);
		return entity;
	}

	public void Update(T entity)
	{
		uow.Entry(entity).State = EntityState.Modified;
	}

	public void Delete(T entity)
	{
    baseEntity.Remove(entity);
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
		return await baseEntity.Where(x => x.Id.Equals(id)).ExecuteDeleteAsync(cancellationToken);
	}
}
