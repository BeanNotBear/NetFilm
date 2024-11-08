using AutoMapper;
using NetFilm.Application.DTOs.MovieDTOs;
using NetFilm.Application.Exceptions;
using NetFilm.Application.Interfaces;
using NetFilm.Domain.Common;
using NetFilm.Domain.Entities;
using NetFilm.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Infrastructure.Services
{
	public class MovieService : IMovieService
	{
		private readonly IMovieRepository _movieRepository;
		private readonly IMapper _mapper;

		public MovieService(IMovieRepository movieRepository, IMapper mapper)
		{
			_movieRepository = movieRepository;
			_mapper = mapper;
		}

		public async Task<MovieDto> AddMovieAsync(string movieName, string movieUrl)
		{
			var movieDomain = new Movie()
			{
				Name = movieName,
				Movie_Url = movieUrl.CreateUrl(),
				Status = MovieStatus.Daft,
				Allowing_Age = 0,
				Average_Star = 0,
				CountryId = Guid.Parse("fe68d192-8239-4a1c-b0d7-601552e7891a"),
				Description = string.Empty,
				Duration = 0,
				Quality = Quality.FullHD,
				Thumbnail = string.Empty,
				Release_Date = DateTime.Now,
				IsDelete = false,
				TotalViews = 0,
			};
			var createdMovieDomain = await _movieRepository.AddAsync(movieDomain);
			var createdMovieDto = _mapper.Map<MovieDto>(createdMovieDomain);
			return createdMovieDto;
		}

		public async Task<IEnumerable<MovieDto>> GetAllAsync()
		{
			var movieDomains =  await _movieRepository.GetAllAsync();
			var movieDtos = _mapper.Map<IEnumerable<MovieDto>>(movieDomains);
			return movieDtos;
		}

		public async Task<MovieDto> GetByIdAsync(Guid id)
		{
			var isExisted = await _movieRepository.ExistsAsync(id);
			if(!isExisted)
			{
				throw new NotFoundException($"Can not found movie with id: {id}");
			}
			var movieDomain = await _movieRepository.GetByIdAsync(id);
			var movieDto = _mapper.Map<MovieDto>(movieDomain);
			return movieDto;
		}

		public async Task<MovieDto> UpdateMovieAsync(Guid id, AddMovieRequestDto addMovieRequestDto)
		{
			await IsExsited(id);
			var movieDomain = _mapper.Map<Movie>(addMovieRequestDto);
			List<MovieCategory> movieCate = new List<MovieCategory>();
			foreach (var category in addMovieRequestDto.CategoryIds)
			{
				movieCate.Add(new MovieCategory { CategoryId = category, MovieId = id });
			}
			movieDomain.MovieCategories = movieCate;
			var updatedMovieDomain = await _movieRepository.UpdateDetails(id, movieDomain);
			var updatedMovieDto = _mapper.Map<MovieDto>(updatedMovieDomain);
			return updatedMovieDto;
		}

		public async Task<MovieDto> UpdateThumbnailAsync(Guid id, string thumbnail)
		{
			await IsExsited(id);
			var updatedMovieDomain = await _movieRepository.UpddateThumbnail(id, thumbnail);
			var updatedMovieDto = _mapper.Map<MovieDto>(updatedMovieDomain);
			return updatedMovieDto;
		}

		private async Task IsExsited(Guid id)
		{
			var isExisted = await _movieRepository.ExistsAsync(id);
			if (!isExisted)
			{
				throw new ExistedEntityException($"Movie with id {id} is already existed!");
			}
		}
	}
}
