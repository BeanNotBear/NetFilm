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
    public class ParticipantRepository : BaseRepository<Participant, Guid>, IParticipantRepository
    {
        private readonly NetFilmDbContext _context;
        //private readonly IParticipantRepository _participantRepository;
        public ParticipantRepository(NetFilmDbContext context) : base(context)
        {
            _context = context;
           // _participantRepository = participantRepository;
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            var participant = await _context.Participants.FirstOrDefaultAsync(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
            return participant != null;
        }

        public async Task<Participant> SoftDeleteAsync(Guid id)
        {
            var entity = await _context.Participants.FindAsync(id);
            entity.IsDelete = true;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
