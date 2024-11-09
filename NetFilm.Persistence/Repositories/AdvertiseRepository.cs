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
    }
}
