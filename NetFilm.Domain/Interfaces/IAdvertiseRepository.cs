using NetFilm.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Domain.Interfaces
{
    public interface IAdvertiseRepository : IBaseRepository<Advertise,Guid>
    {
        Task<IEnumerable<Advertise>> GetAllAdvertisesAsync();
        Task<Advertise> GetAdvertiseByIdAsync(Guid id);
        Task<IEnumerable<Advertise>> GetAdvertisePagedResultAsync(int pageSize, int pageIndex, string searchTerm, string sortBy, bool ascending);
    }
}
