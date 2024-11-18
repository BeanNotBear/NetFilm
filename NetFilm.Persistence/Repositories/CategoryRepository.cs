using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetFilm.Domain.Common;
using NetFilm.Domain.Entities;
using NetFilm.Domain.Interfaces;
using NetFilm.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Persistence.Repositories
{
    public class CategoryRepository : BaseRepository<Category, Guid>, ICategoryRepository
    {
        private readonly NetFilmDbContext _context;
        public CategoryRepository(NetFilmDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<bool> ExistsByNameAsync(string name)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
            return category != null;
        }

        public async Task<IEnumerable<Category>> GetCategoryPagedResultAsync(int pageSize,int pageIndex,string searchTerm,string sortBy,bool ascending)
        {

            // Start with all users as IQueryable
            IQueryable<Category> query = _context.Categories.AsQueryable();

            // Apply search filters if searchTerm is provided
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var search = searchTerm.Trim().ToLower();
                query = query.Where(u =>
                    u.Name.ToLower().Contains(searchTerm)
                );
            }

            // Apply sorting
            query = sortBy?.ToLower() switch
            {
                "name" => ascending
                    ? query.OrderBy(u => u.Name)
                    : query.OrderByDescending(u => u.Name),
                _ => query.OrderBy(u => u.Name) // default sorting
            };

            // Apply pagination
            var categories = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();


            return categories;
        }
    }
}
