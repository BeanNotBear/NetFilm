using Microsoft.EntityFrameworkCore;
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

        public async Task<Category> GetByName(string name)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
        }
    }
}
