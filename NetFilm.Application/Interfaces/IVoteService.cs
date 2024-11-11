using NetFilm.Application.DTOs.VoteDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Application.Interfaces
{
    public interface IVoteService
    {
        Task<IEnumerable<VoteDto>> GetAll();
        Task<VoteDto> GetById(Guid id);
        Task<VoteDto> Add(AddVoteRequestDTO addVoteRequestDTO);
        Task<bool> HardDelete(Guid id);
        Task<VoteDto> SoftDelete(Guid id);
        Task<VoteDto> Update(Guid id, UpdateVoteRequestDTO updateVoteRequestDTO);
    }
}
