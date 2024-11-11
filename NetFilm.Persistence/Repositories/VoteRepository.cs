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
    public class VoteRepository : BaseRepository<Vote, Guid>, IVoteRepository
    {
        private readonly NetFilmDbContext _context;
        //private readonly IVoteRepository _voteRepository;
        public VoteRepository(NetFilmDbContext context) : base(context)
        {
            //_voteRepository = voteRepository;
            _context = context;
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return true;
        }

        public async Task<Vote> SoftDeleteAsync(Guid id)
        {
            var entity = await _context.Votes.FindAsync(id);
            entity.IsDelete = true;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
