using UnusualSuspect.Entities;

namespace UnusualSuspect.DataLayer.Contracts;

/// <summary>
/// Source: My reference app https://github.com/dotnet-architecture/eShopOnWeb
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IAsyncRepository<T> where T : BaseEntity
{
	Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

	Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default);

	Task<IReadOnlyList<T>> ListAllAsync(int perPage, int page, CancellationToken cancellationToken = default);

	//Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

	T Add(T entity);

	void Update(T entity);

	void Delete(T entity);
	void DeleteById(int id);
	Task<int> ExecuteDeleteByIdAsync(int id, CancellationToken cancellationToken = default);
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

	//Task<int> CountAsync(ISpecification<T> spec);
}
