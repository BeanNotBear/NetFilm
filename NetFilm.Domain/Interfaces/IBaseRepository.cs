using NetFilm.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Domain.Interfaces
{
	/// <summary>
	/// This interface is base repository. 
	/// </summary>
	/// <typeparam name="TEntity">Entity</typeparam>
	/// <typeparam name="TId">Id of entity</typeparam>
	public interface IBaseRepository<TEntity, TId> where TEntity : class
	{
		/// <summary>
		/// Gets all entities asynchronously
		/// </summary>
		/// <returns>IEnumerable of entities</returns>
		Task<IEnumerable<TEntity>> GetAllAsync();

		/// <summary>
		/// Retrieves a paged result of <typeparamref name="TEntity"/> from the repository based on the specified filters, sorting, and pagination settings.
		/// </summary>
		/// <param name="filter">
		/// An optional filter expression to apply to the data. If null, all records are retrieved.
		/// </param>
		/// <param name="orderBy">
		/// An optional sorting function that determines the order of the results.
		/// Pass a function that takes an <see cref="IReadOnlyList{TEntity}"/> and returns an <see cref="IOrderedQueryable{TEntity}"/>.
		/// If null, the default ordering is applied.
		/// </param>
		/// <param name="includes">
		/// A comma-separated list of related entities to include in the result, using Entity Framework's `Include` syntax.
		/// For example, "Orders,Customer". If left empty, no related entities are included.
		/// </param>
		/// <param name="pageIndex">
		/// The 1-based index of the page to retrieve. Defaults to 1, representing the first page.
		/// </param>
		/// <param name="pageSize">
		/// The number of items per page. Defaults to 10 and is capped at a maximum size (e.g., 25) to prevent excessive data retrieval.
		/// </param>
		/// <returns>
		/// A <see cref="Task{TResult}"/> representing the asynchronous operation, with a <see cref="PagedResult{TEntity}"/> as the result.
		/// This result contains the filtered and paged list of <typeparamref name="TEntity"/> objects, as well as pagination metadata
		/// like total items and total pages.
		/// </returns>
		/// <remarks>
		/// Use this method to retrieve a specific page of data with optional filtering, ordering, and eager loading of related entities.
		/// This is particularly useful for displaying data in paginated lists, as it reduces the amount of data retrieved and improves performance.
		/// </remarks>
		Task<PagedResult<TEntity>> GetPagedResultAsync(
			Expression<Func<TEntity, bool>>? filter = null,
			Func<IReadOnlyList<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
			string includes = "",
			int pageIndex = 1,
			int pageSize = 10);

		/// <summary>
		/// Gets entity by id asynchronously
		/// </summary>
		/// <param name="id">Entity identifier</param>
		/// <returns>Entity or null if not found</returns>
		Task<TEntity> GetByIdAsync(TId id);

		/// <summary>
		/// Adds new entity asynchronously
		/// </summary>
		/// <param name="entity">Entity to add</param>
		/// <returns>Added entity</returns>
		Task<TEntity> AddAsync(TEntity entity);

		/// <summary>
		/// Updates existing entity asynchronously
		/// </summary>
		/// <param name="entity">Entity to update</param>
		/// <returns>Updated entity</returns>
		Task<TEntity> UpdateAsync(TEntity entity);

		/// <summary>
		/// Deletes entity by id asynchronously
		/// </summary>
		/// <param name="id">Entity identifier</param>
		/// <returns>True if deleted successfully, false otherwise</returns>
		Task<bool> DeleteAsync(TId id);

		/// <summary>
		/// Checks if entity exists asynchronously
		/// </summary>
		/// <param name="id">Entity identifier</param>
		/// <returns>True if exists, false otherwise</returns>
		Task<bool> ExistsAsync(TId id);

		/// <summary>
		/// Gets count of entities asynchronously
		/// </summary>
		/// <returns>Number of entities</returns>
		Task<int> CountAsync();

		/// <summary>
		/// Gets count of entities matching predicate asynchronously
		/// </summary>
		/// <param name="predicate">Filter expression</param>
		/// <returns>Number of matching entities</returns>
		Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
	}
}
