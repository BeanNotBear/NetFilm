using NetFilm.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Domain.Interfaces
{
    public interface IVoteRepository : IBaseRepository<Vote, Guid>
    {
        Task<bool> ExistsByNameAsync(string name);
        Task<Vote> SoftDeleteAsync(Guid id);
        Task<Vote?> GetVoteAsync(Guid movieId, Guid userId);
        Task<int> CountMovie(Guid movieId);
        Task<bool> CheckExists(Guid moviewId, Guid userId);
    }
}
