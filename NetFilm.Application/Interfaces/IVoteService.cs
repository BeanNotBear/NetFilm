using NetFilm.Application.DTOs.VoteDtos;
using NetFilm.Application.Model;
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
        Task<ResultModel<VoteDto>> Add(AddVoteRequestDTO addVoteRequestDTO);
        Task<bool> HardDelete(Guid id);
        Task<VoteDto> SoftDelete(Guid id);
        Task<ResultModel<VoteDto>> Update(UpdateVoteRequestDTO updateVoteRequestDto);
        Task<ResultModel<VoteDto>> DeleteAsync(Guid id);
    }
}
