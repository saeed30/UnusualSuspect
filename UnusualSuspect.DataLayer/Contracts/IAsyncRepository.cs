using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.DataLayer.Contracts;

public interface IAsyncRepository<T> : IAsyncRepository<T, int> where T : IEntity<int>
{

}
/// <summary>
/// Source: My reference app https://github.com/dotnet-architecture/eShopOnWeb
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TY"></typeparam>
public interface IAsyncRepository<T, TY> where T : IEntity<TY>
{
	Task<T?> GetByIdAsync(TY id, CancellationToken cancellationToken = default);

	IQueryable<T> GetAll();
	Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default);

	Task<IReadOnlyList<T>> ListAllAsync(int perPage, int page, CancellationToken cancellationToken = default);

	//Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

	T Add(T entity);

	void Update(T entity);

	void Delete(T entity);
	void DeleteRange(List<T> entity);
	void DeleteById(TY id);
	Task<int> ExecuteDeleteByIdAsync(TY id, CancellationToken cancellationToken = default);
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

	//Task<int> CountAsync(ISpecification<T> spec);
}
