using AutoMapper;
using NetFilm.Application.DTOs.CountryDTOs;
using NetFilm.Application.DTOs.ParticipantDTOs;
using NetFilm.Application.Exceptions;
using NetFilm.Application.Interfaces;
using NetFilm.Domain.Entities;
using NetFilm.Domain.Interfaces;

namespace NetFilm.Infrastructure.Services
{
    public class ParticipantService : IParticipantService

    {
        private readonly IParticipantRepository _participantRepository;
        private readonly IMapper _mapper;


        public ParticipantService(IParticipantRepository participantRepository, IMapper mapper)
        {
            _mapper = mapper;
            _participantRepository = participantRepository;
        }

        /// <summary>
        /// Add country to Database
        /// </summary>
        /// <param name="addCountryRequestDto">Add country request</param>
        /// <returns>created country</returns>
        public async Task<ParticipantDto> Add(AddParticipantRequestDto addParticipantRequestDto)
        {
            var isExisted = await _participantRepository.ExistsByNameAsync(addParticipantRequestDto.Name);
            if (isExisted)
            {
                throw new ExistedEntityException($"{addParticipantRequestDto.Name} is already existed!");
            }
            var participantDomain = _mapper.Map<Participant>(addParticipantRequestDto);

            var createdParticipantDomain = await _participantRepository.AddAsync(participantDomain);
            var createdParticipantDto = _mapper.Map<ParticipantDto>(createdParticipantDomain);
            return createdParticipantDto;
        }

        /// <summary>
        /// Hard delete country
        /// </summary>
        /// <param name="id">country id</param>
        /// <returns>boolean</returns>
        /// <exception cref="NotFoundException">Throw when not found</exception>
        /// <exception cref="Exception">Throw when can not delete</exception>
        public async Task<bool> HardDelete(Guid id)
        {
            var isExisted = await _participantRepository.ExistsAsync(id);
            if (!isExisted)
            {
                throw new NotFoundException($"Cannot find participant with Id: {id}");
            }
            var isDeleted = await _participantRepository.DeleteAsync(id);
            if (!isDeleted)
            {
                throw new Exception("Something went wrong!");
            }
            return true;
        }


        /// <summary>
        /// Get all countrys
        /// </summary>
        /// <returns>List of countryDtos</returns>
        /// <summary>
        /// Get all participants
        /// </summary>
        /// <returns>List of ParticipantDtos</returns>
        public async Task<IEnumerable<ParticipantDto>> GetAll()
        {
            var participantDomains = await _participantRepository.GetAllAsync();
            var participantDtos = _mapper.Map<IEnumerable<ParticipantDto>>(participantDomains);
            return participantDtos;
        }


        /// <summary>
        /// Get country by Id
        /// </summary>
        /// <param name="id">Id of country</param>
        /// <returns>Country Dto</returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <summary>
        /// Get participant by Id
        /// </summary>
        /// <param name="id">Id of participant</param>
        /// <returns>Participant Dto</returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<ParticipantDto> GetById(Guid id)
        {
            var participantDomain = await _participantRepository.GetByIdAsync(id);
            if (participantDomain == null)
            {
                throw new NotFoundException($"Cannot find participant with Id {id}");
            }
            var participantDto = _mapper.Map<ParticipantDto>(participantDomain);
            return participantDto;
        }


        /// <summary>
        /// Soft delete country
        /// </summary>
        /// <param name="id"> country id</param>
        /// <returns>country dto</returns>
        /// <summary>
        /// Soft delete participant
        /// </summary>
        /// <param name="id">Participant id</param>
        /// <returns>Participant dto</returns>
        public async Task<ParticipantDto> SoftDelete(Guid id)
        {
            var isExisted = await _participantRepository.ExistsAsync(id);
            if (!isExisted)
            {
                throw new NotFoundException($"Cannot find participant with Id: {id}");
            }
            var participantDomain = await _participantRepository.SoftDeleteAsync(id);
            var participantDto = _mapper.Map<ParticipantDto>(participantDomain);
            return participantDto;
        }


        /// <summary>
        /// Update country
        /// </summary>
        /// <param name="updateCountryRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ParticipantDto> Update(Guid id, UpdateParticipantRequestDto updateParticipantRequestDto)
        {
            var participantDomain = await _participantRepository.GetByIdAsync(id);
            if (participantDomain == null)
            {
                throw new NotFoundException($"Cannot find participant with Id: {id}");
            }
            participantDomain.Name = updateParticipantRequestDto.Name;
            participantDomain.RoleInMovie = updateParticipantRequestDto.RoleInMovie;

            var updatedParticipantDomain = await _participantRepository.UpdateAsync(participantDomain);
            if (updatedParticipantDomain == null)
            {
                throw new Exception("Something went wrong!");
            }
            var updatedParticipantDto = _mapper.Map<ParticipantDto>(updatedParticipantDomain);
            return updatedParticipantDto;
        }

    }
}