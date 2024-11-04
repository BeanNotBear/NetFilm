using NetFilm.Domain.Common;
using NetFilm.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Persistence.Repositories
{
	/// <summary>
	/// Base repository
	/// </summary>
	/// <typeparam name="TEntity">Entity</typeparam>
	/// <typeparam name="TId">Data type Id of entity</typeparam>
	public class BaseRepository<TEntity, TId> : IBaseRepository<TEntity, TId> where TEntity : class
	{
		public Task<TEntity> AddAsync(TEntity entity)
		{
			throw new NotImplementedException();
		}

		public Task<int> CountAsync()
		{
			throw new NotImplementedException();
		}

		public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteAsync(TId id)
		{
			throw new NotImplementedException();
		}

		public Task<bool> ExistsAsync(TId id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<TEntity>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task<TEntity> GetByIdAsync(TId id)
		{
			throw new NotImplementedException();
		}

		public Task<PagedResult<TEntity>> GetPagedResultAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IReadOnlyList<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includes = "", int pageIndex = 1, int pageSize = 10)
		{
			throw new NotImplementedException();
		}

		public Task<TEntity> UpdateAsync(TEntity entity)
		{
			throw new NotImplementedException();
		}
	}
}
