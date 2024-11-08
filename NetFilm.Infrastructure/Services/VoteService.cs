using AutoMapper;
using NetFilm.Application.DTOs.CountryDTOs;
using NetFilm.Application.DTOs.ParticipantDTOs;
using NetFilm.Application.DTOs.VoteDtos;
using NetFilm.Application.Exceptions;
using NetFilm.Application.Interfaces;
using NetFilm.Domain.Entities;
using NetFilm.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Infrastructure.Services
{
    public class VoteService : IVoteService
    {
        private readonly IVoteRepository _voteRepository;
        private readonly IMapper _mapper;

        public VoteService(IVoteRepository voteRepository, IMapper mapper)
        {
            _voteRepository = voteRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Add country to Database
        /// </summary>
        /// <param name="addCountryRequestDto">Add country request</param>
        /// <returns>created country</returns>
        public async Task<CountryDto> Add(AddVoteRequestDTO addVoteRequestDTO)
        {
            var voteDomain = _mapper.Map<Vote>(addVoteRequestDTO);
            var createdVoteDomain = await _voteRepository.AddAsync(voteDomain);
            var createdVoteDto = _mapper.Map<CountryDto>(createdVoteDomain);
            return createdVoteDto;
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
            var isExisted = await _voteRepository.ExistsAsync(id);
            if (!isExisted)
            {
                throw new NotFoundException($"Cannot find vote with Id: {id}");
            }
            var isDeleted = await _voteRepository.DeleteAsync(id);
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
        /// Get all votes
        /// </summary>
        /// <returns>List of VoteDtos</returns>
        public async Task<IEnumerable<VoteDto>> GetAll()
        {
            var voteDomains = await _voteRepository.GetAllAsync();
            var voteDtos = _mapper.Map<IEnumerable<VoteDto>>(voteDomains);
            return voteDtos;
        }


        /// <summary>
        /// Get country by Id
        /// </summary>
        /// <param name="id">Id of country</param>
        /// <returns>Country Dto</returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <summary>
        /// Get vote by Id
        /// </summary>
        /// <param name="id">Id of vote</param>
        /// <returns>Vote Dto</returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<VoteDto> GetById(Guid id)
        {
            var voteDomain = await _voteRepository.GetByIdAsync(id);
            if (voteDomain == null)
            {
                throw new NotFoundException($"Cannot find vote with Id {id}");
            }
            var voteDto = _mapper.Map<VoteDto>(voteDomain);
            return voteDto;
        }


        /// <summary>
        /// Soft delete country
        /// </summary>
        /// <param name="id"> country id</param>
        /// <returns>country dto</returns>

        /// <summary>
        /// Soft delete vote
        /// </summary>
        /// <param name="id">Vote id</param>
        /// <returns>Vote dto</returns>
        public async Task<VoteDto> SoftDelete(Guid id)
        {
            var isExisted = await _voteRepository.ExistsAsync(id);
            if (!isExisted)
            {
                throw new NotFoundException($"Cannot find vote with Id: {id}");
            }
            var voteDomain = await _voteRepository.SoftDeleteAsync(id);
            var voteDto = _mapper.Map<VoteDto>(voteDomain);
            return voteDto;
        }


        /// <summary>
        /// Update country
        /// </summary>
        /// <param name="updateCountryRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <summary>
        /// Update vote
        /// </summary>
        /// <param name="id">Vote id</param>
        /// <param name="updateVoteRequestDto">Update request data</param>
        /// <returns>Updated vote dto</returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<VoteDto> Update(Guid id, UpdateVoteRequestDTO updateVoteRequestDto)
        {
            var voteDomain = await _voteRepository.GetByIdAsync(id);
            if (voteDomain == null)
            {
                throw new NotFoundException($"Cannot find vote with Id: {id}");
            }

            var updatedVoteDomain = await _voteRepository.UpdateAsync(voteDomain);
            if (updatedVoteDomain == null)
            {
                throw new Exception("Something went wrong!");
            }
            var updatedVoteDto = _mapper.Map<VoteDto>(updatedVoteDomain);
            return updatedVoteDto;
        }

        Task<VoteDto> IVoteService.Add(AddVoteRequestDTO addVoteRequestDTO)
        {
            throw new NotImplementedException();
        }
    }
}
