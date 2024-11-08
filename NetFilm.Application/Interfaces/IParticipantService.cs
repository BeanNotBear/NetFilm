
using NetFilm.Application.DTOs.CountryDTOs;
using NetFilm.Application.DTOs.ParticipantDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Application.Interfaces
{
    public interface IParticipantService
    {
        Task<IEnumerable<ParticipantDto>> GetAll();
        Task<ParticipantDto> GetById(Guid id);
        Task<ParticipantDto> Add(AddParticipantRequestDto adParticipantRequestDto);
        Task<bool> HardDelete(Guid id);
        Task<ParticipantDto> SoftDelete(Guid id);
        Task<ParticipantDto> Update(Guid id, UpdateParticipantRequestDto updateParticipantRequestDto);

    }
}
