﻿using Microsoft.EntityFrameworkCore;
using NetFilm.Domain.Common;
using NetFilm.Domain.Interfaces;
using NetFilm.Persistence.Data;
using System.Linq.Expressions;

namespace NetFilm.Persistence.Repositories
{
	/// <summary>
	/// Base repositoy for all sub repository
	/// </summary>
	/// <typeparam name="TEntity">Data type of entity</typeparam>
	/// <typeparam name="TId">Data type of key</typeparam>
	public class BaseRepository<TEntity, TId> : IBaseRepository<TEntity, TId> where TEntity : class
	{
		private readonly NetFilmDbContext _context;
		protected readonly DbSet<TEntity> _dbSet;

		public BaseRepository(NetFilmDbContext context)
		{
			_context = context;
			_dbSet = context.Set<TEntity>();
		}

		public virtual async Task<TEntity> AddAsync(TEntity entity)
		{
			await _dbSet.AddAsync(entity);
			await _context.SaveChangesAsync();
			return entity;
		}

		public virtual async Task<int> CountAsync()
		{
			return await _dbSet.CountAsync();
		}

		public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return await _dbSet.CountAsync(predicate);
		}

		public virtual async Task<bool> DeleteAsync(TId id)
		{
			var entity = await _dbSet.FindAsync(id);
			if (entity == null)
				return false;

			_dbSet.Remove(entity);
			await _context.SaveChangesAsync();
			return true;
		}

		public virtual async Task<bool> ExistsAsync(TId id)
		{
			var entity = await _dbSet.FindAsync(id);
			return entity != null;
		}

		public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
		{
			return await _dbSet.ToListAsync();
		}

		public virtual async Task<TEntity> GetByIdAsync(TId id)
		{
			return await _dbSet.FindAsync(id);
		}

		public virtual async Task<PagedResult<TEntity>> GetPagedResultAsync(
			Expression<Func<TEntity, bool>>? filter = null,
			Func<IReadOnlyList<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
			string includes = "",
			int pageIndex = 1,
			int pageSize = 10)
		{
			IQueryable<TEntity> query = _dbSet;

			// Apply filtering
			if (filter != null)
			{
				query = query.Where(filter);
			}

			// Include related entities
			if (!string.IsNullOrWhiteSpace(includes))
			{
				foreach (var include in includes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(include.Trim());
				}
			}

			// Get total count
			var totalItems = await query.CountAsync();

			// Apply ordering if specified
			if (orderBy != null)
			{
				var list = await query.ToListAsync();
				query = orderBy(list).AsQueryable();
			}

			// Apply pagination
			var items = query
				.Skip((pageIndex - 1) * pageSize)
				.Take(pageSize)
				.ToList();

			return new PagedResult<TEntity>(items, totalItems, pageIndex, pageSize);
		}

		public virtual async Task<TEntity> UpdateAsync(TEntity entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
			await _context.SaveChangesAsync();
			return entity;
		}
	}
}