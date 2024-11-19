using AutoMapper;
using NetFilm.Application.DTOs.AdvertiseDTOs;
using NetFilm.Application.DTOs.CommentDTOs;
using NetFilm.Application.Exceptions;
using NetFilm.Application.Interfaces;
using NetFilm.Domain.Common;
using NetFilm.Domain.Entities;
using NetFilm.Domain.Interfaces;
using NetFilm.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Infrastructure.Services
{
    public class AdvertiseService : IAdvertiseService
    {
        private readonly IAdvertiseRepository _advertiseRepository;
        private readonly IMapper _mapper;

        public AdvertiseService(IAdvertiseRepository advertiseRepository, IMapper mapper)
        {
            _advertiseRepository = advertiseRepository;
            _mapper = mapper;
        }

        public async Task<AdvertiseDto> AddAdvertise(AddAdvertiseDto addAdvertiseDto, string image)
        {
            var advertise = _mapper.Map<Advertise>(addAdvertiseDto);
            advertise.Image = image;
            advertise.Id = Guid.NewGuid();
            await _advertiseRepository.AddAsync(advertise);
            return _mapper.Map<AdvertiseDto>(await _advertiseRepository.GetAdvertiseByIdAsync(advertise.Id));
        }

        public async Task<bool> HardDelete(Guid id)
        {
            var isExisted = await _advertiseRepository.ExistsAsync(id);
            if(!isExisted)
            {
                throw new NotFoundException($"Can not found advertise with Id {id}");
            }
            var isDeleted =  await _advertiseRepository.DeleteAsync(id);
            if(!isDeleted)
            {
                throw new Exception("Some things went wrong!");
            }
            return true;
        }

        public async Task<AdvertiseDto> GetAdvertiseById(Guid id)
        {
            var advertise = await _advertiseRepository.GetAdvertiseByIdAsync(id);
            if(advertise == null)
            {
                throw new NotFoundException($"Can not found advertise with Id {id}");
            }
            return _mapper.Map<AdvertiseDto>(advertise);
        }

        public async Task<IEnumerable<AdvertiseDto>> GetAllAdvertises()
        {
            var advertises = await _advertiseRepository.GetAllAdvertisesAsync();
            return _mapper.Map<IEnumerable<AdvertiseDto>>(advertises);
        }

        public async Task<AdvertiseDto> UpdateAdvertise(Guid id, UpdateAdvertiseDto updateAdvertiseDto, string image)
        {
            var advertise = await _advertiseRepository.GetAdvertiseByIdAsync(id);
            if (advertise == null)
            {
                throw new NotFoundException($"Can not found advertise with Id {id}");
            }
            _mapper.Map(updateAdvertiseDto,advertise);
            if (!image.Equals("/"))
            {
                advertise.Image = image;
            }
            await _advertiseRepository.UpdateAsync(advertise);
            return _mapper.Map<AdvertiseDto>(await _advertiseRepository.GetAdvertiseByIdAsync(advertise.Id));
        }

        public async Task<PagedResult<AdvertiseDto>> GetAdvertisePagedResult(AdvertiseQueryParams queryParams)
        {
            // Validate
            queryParams.Validate();

            var advertises = await _advertiseRepository.GetAdvertisePagedResultAsync(queryParams.PageSize, queryParams.PageIndex, queryParams.SearchTerm, queryParams.SortBy, queryParams.Ascending);
            var advertisesDto = _mapper.Map<IEnumerable<AdvertiseDto>>(advertises);
            var totalItem = await _advertiseRepository.CountAsync();
            return new PagedResult<AdvertiseDto>(advertisesDto, totalItem, queryParams.PageIndex, queryParams.PageSize);
        }

        public async Task<AdvertiseDto> GetRandomAdvertise()
        {
            var advertises = await _advertiseRepository.GetAllAdvertisesAsync();
            Random random = new Random();
            int randomNumber = random.Next(advertises.Count());
            return _mapper.Map<AdvertiseDto>(advertises.ElementAt(randomNumber));
        }
    }
}
