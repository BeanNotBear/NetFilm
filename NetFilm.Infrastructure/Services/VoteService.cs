using AutoMapper;
using NetFilm.Application.DTOs.CountryDTOs;
using NetFilm.Application.DTOs.ParticipantDTOs;
using NetFilm.Application.DTOs.VoteDtos;
using NetFilm.Application.Exceptions;
using NetFilm.Application.Interfaces;
using NetFilm.Application.Model;
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
        private readonly IMovieRepository _movieRepository;

        public VoteService(IVoteRepository voteRepository, IMapper mapper, IMovieRepository movieRepository)
        {
            _voteRepository = voteRepository;
            _mapper = mapper;
            _movieRepository = movieRepository;
        }

        /// <summary>
        /// Add country to Database
        /// </summary>
        /// <param name="addCountryRequestDto">Add country request</param>
        /// <returns>created country</returns>
       

        public async Task<ResultModel<VoteDto>> Add(AddVoteRequestDTO addVoteRequestDTO)
        {
            try
            {
                if (await _voteRepository.CheckExists(addVoteRequestDTO.MovieId, addVoteRequestDTO.UserId))
                {
                    throw new Exception("You have already voted for this movie");
                }
                var movie = await _movieRepository.GetByIdAsync(addVoteRequestDTO.MovieId)
                    ?? throw new NotFoundException($"Cannot find movie with Id: {addVoteRequestDTO.MovieId}");
                var vote = _mapper.Map<Vote>(addVoteRequestDTO);
                var countMovie = await _voteRepository.CountMovie(addVoteRequestDTO.MovieId);
                countMovie += 1;
                movie.Average_Star = (movie.Average_Star * (countMovie - 1) + addVoteRequestDTO.Star) / countMovie;
                await _voteRepository.AddAsync(vote);
                await _movieRepository.UpdateAsync(movie);
                var voteDto = _mapper.Map<VoteDto>(vote);
                return new ResultModel<VoteDto>
                {
                    Data = voteDto,
                    IsSuccess = true,

                };
            }
            catch (Exception ex) 
            {
                return new ResultModel<VoteDto>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
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
        public async Task<ResultModel<VoteDto>> Update(UpdateVoteRequestDTO updateVoteRequestDto)
        {
            try
            {
                if (!await _voteRepository.CheckExists(updateVoteRequestDto.MovieId, updateVoteRequestDto.UserId))
                {
                    throw new Exception("You have not voted for this movie");
                }
                var movie = await _movieRepository.GetByIdAsync(updateVoteRequestDto.MovieId)
                            ?? throw new Exception("Movie not found");

                var vote = await _voteRepository.GetVoteAsync(updateVoteRequestDto.MovieId, updateVoteRequestDto.UserId)
                           ?? throw new Exception("Vote not found");
                var oldVote = vote.Star;
                var countMovie = await _voteRepository.CountMovie(updateVoteRequestDto.MovieId);
                vote.Star = updateVoteRequestDto.Star;
                movie.Average_Star = (movie.Average_Star * countMovie - oldVote + updateVoteRequestDto.Star) / countMovie;
                await _voteRepository.UpdateAsync(vote);
                await _movieRepository.UpdateAsync(movie);
                var voteDto = _mapper.Map<VoteDto>(vote);
                return new ResultModel<VoteDto>
                {
                    Data = voteDto,
                    IsSuccess = true,

                };
            }
            catch (Exception ex)
            {
                return new ResultModel<VoteDto>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<ResultModel<VoteDto>> DeleteAsync(Guid id)
        {
            try
            {
                var vote = await _voteRepository.GetByIdAsync(id) ?? throw new Exception("Vote not found");
                var movie = await _movieRepository.GetByIdAsync(vote.MovieId) ?? throw new Exception("Movie not found");
                var oldVote = vote.Star;
                await _voteRepository.DeleteAsync(vote.Id);
                var countMovie = await _voteRepository.CountMovie(vote.MovieId);
                if (countMovie > 0)
                {
                    countMovie -= 1;
                    movie.Average_Star = (movie.Average_Star * (countMovie + 1) - oldVote) / countMovie;
                }
                else
                {
                    movie.Average_Star = 0;
                }
                await _movieRepository.UpdateAsync(movie);
                return new ResultModel<VoteDto>
                {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new ResultModel<VoteDto>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }   
        }

    }
}
