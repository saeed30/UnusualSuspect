using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.Entities;

namespace UnusualSuspect.DataLayer.Common
{
	internal class DapperRepository<T> : IAsyncRepository<T> where T : BaseEntity
	{
		public Task<T> AddAsync(T entity, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(T entity, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<T> GetByIdAsync(int id, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<IReadOnlyList<T>> ListAllAsync(int perPage, int page, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(T entity, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
