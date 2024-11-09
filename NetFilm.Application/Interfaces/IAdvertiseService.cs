using NetFilm.Application.DTOs.AdvertiseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Application.Interfaces
{
    public interface IAdvertiseService
    {
        Task<IEnumerable<AdvertiseDto>> GetAllAdvertises();
        Task<AdvertiseDto> GetAdvertiseById(Guid id);
        Task<AdvertiseDto> AddAdvertise(AddAdvertiseDto addAdvertiseDto,string image);
        Task<AdvertiseDto> UpdateAdvertise(Guid id,UpdateAdvertiseDto updateAdvertiseDto,string image);
        Task<bool> HardDelete(Guid id);
    }
}
