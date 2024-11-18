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
    public class AdvertiseRepository : BaseRepository<Advertise, Guid>, IAdvertiseRepository
    {
        public readonly NetFilmDbContext _context;
        public AdvertiseRepository(NetFilmDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Advertise>> GetAllAdvertisesAsync()
        {
            var advertises = await _context.Advertises.Include(a => a.User).ToListAsync();
            return advertises;
        }

        public async Task<Advertise> GetAdvertiseByIdAsync(Guid id)
        {
            var advertises = await _context.Advertises.Include(a =>a.User).FirstOrDefaultAsync(a => a.Id == id);
            return advertises;
        }

        public async Task<IEnumerable<Advertise>> GetAdvertisePagedResultAsync(int pageSize, int pageIndex, string searchTerm, string sortBy, bool ascending)
        {

            // Start with all users as IQueryable
            IQueryable<Advertise> query = _context.Advertises.Include(a => a.User).AsQueryable();

            // Apply search filters if searchTerm is provided
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var search = searchTerm.Trim().ToLower();
                query = query.Where(u =>
                    u.Content.ToLower().Contains(searchTerm)
                    || u.User.LastName.ToLower().Contains(searchTerm)
                    || u.Title.ToLower().Contains(searchTerm)
                );
            }

            // Apply sorting
            query = sortBy?.ToLower() switch
            {
                "content" => ascending
                    ? query.OrderBy(u => u.Content)
                    : query.OrderByDescending(u => u.Content),
                "createby" => ascending
                ? query.OrderBy(u => u.User.UserName)
                : query.OrderByDescending(u => u.User.UserName),
                "title" => ascending
                ? query.OrderBy(u => u.Title)
                : query.OrderByDescending(u => u.Title),
                _ => query.OrderBy(u => u.Title) // default sorting
            };

            // Apply pagination
            var advertises = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();


            return advertises;
        }
    }
}
