using AutoMapper;
using Microsoft.Extensions.Configuration;
using NetFilm.Application.DTOs.MovieDTOs;
using NetFilm.Application.Exceptions;
using NetFilm.Application.Interfaces;
using NetFilm.Domain.Common;
using NetFilm.Domain.Entities;
using NetFilm.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Infrastructure.Services
{
	public class MovieService : IMovieService
	{
		private readonly IMovieRepository _movieRepository;
		private readonly IMapper _mapper;
		private readonly IConfiguration _configuration;

		public MovieService(IMovieRepository movieRepository, IMapper mapper, IConfiguration configuration)
		{
			_movieRepository = movieRepository;
			_mapper = mapper;
			_configuration = configuration;
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
			var movieDomains = await _movieRepository.GetAllAsync();
			var movieDtos = _mapper.Map<IEnumerable<MovieDto>>(movieDomains);
			return movieDtos;
		}

		public async Task<MovieDto> GetByIdAsync(Guid id)
		{
			var isExisted = await _movieRepository.ExistsAsync(id);
			if (!isExisted)
			{
				throw new NotFoundException($"Can not found movie with id: {id}");
			}
			var movieDomain = await _movieRepository.GetByIdAsync(id);
			var movieDto = _mapper.Map<MovieDto>(movieDomain);
			return movieDto;
		}

		public async Task<PagedResult<MovieDto>> GetPaging(MovieQueryParam queryParam)
		{
			Expression<Func<Movie, bool>> filter = x => (
				(string.IsNullOrWhiteSpace(queryParam.SearchTerm) || x.Name.Contains(queryParam.SearchTerm) || x.Description.Contains(queryParam.SearchTerm)) &&
				(!queryParam.Status.HasValue || x.Status == queryParam.Status) &&
				(!queryParam.Quality.HasValue || x.Quality == queryParam.Quality) &&
				(!queryParam.AllowingAge.HasValue || x.Allowing_Age >= queryParam.AllowingAge) &&
				(!queryParam.AverageStar.HasValue || x.Average_Star >= queryParam.AverageStar) &&
				(!queryParam.Country.HasValue || x.CountryId == queryParam.Country) &&
				(!queryParam.Category.HasValue || x.MovieCategories.Select(x => x.CategoryId).ToList().Contains(queryParam.Category.Value)) &&
				(!queryParam.ReleaseDate.HasValue || x.Release_Date >= queryParam.ReleaseDate) &&
				(!queryParam.IsDeleted.HasValue || x.IsDelete == queryParam.IsDeleted.Value) &&
				(!queryParam.Participant.HasValue || x.MovieParticipants.Select(x => x.ParticipantId).ToList().Contains(queryParam.Participant.Value))
			);

			Func<IReadOnlyList<Movie>, IOrderedQueryable<Movie>>? orderBy = null;

			if (!string.IsNullOrWhiteSpace(queryParam.SortBy))
			{
				orderBy = movies =>
				{
					var query = movies.AsQueryable();
					return queryParam.SortBy.ToLower() switch
					{
						"name" => queryParam.Ascending
							? query.OrderBy(x => x.Name)
							: query.OrderByDescending(x => x.Name),
						"releasedate" => queryParam.Ascending
							? query.OrderBy(x => x.Release_Date)
							: query.OrderByDescending(x => x.Release_Date),
						"averagestar" => queryParam.Ascending
							? query.OrderBy(x => x.Average_Star)
							: query.OrderByDescending(x => x.Average_Star),
						"allowingage" => queryParam.Ascending
							? query.OrderBy(x => x.Allowing_Age)
							: query.OrderByDescending(x => x.Allowing_Age),
						_ => query.OrderByDescending(x => x.Release_Date) // default sorting
					};
				};
			}

			var pageResultDomain = await _movieRepository.GetPagedResultAsync(filter, orderBy, queryParam.Includes, queryParam.PageIndex, queryParam.PageSize);
			var pageResultDto = _mapper.Map<PagedResult<MovieDto>>(pageResultDomain);
			return pageResultDto;
		}

		public async Task<PagedResult<MovieDto>> GetMoviePaging(MovieQueryParam queryParam)
		{
			Expression<Func<Movie, bool>> filter = x => (
				(string.IsNullOrWhiteSpace(queryParam.SearchTerm) || x.Name.Contains(queryParam.SearchTerm) || x.Description.Contains(queryParam.SearchTerm)) &&
				(!queryParam.Status.HasValue || x.Status == queryParam.Status) &&
				(!queryParam.Quality.HasValue || x.Quality == queryParam.Quality) &&
				(!queryParam.AllowingAge.HasValue || x.Allowing_Age >= queryParam.AllowingAge) &&
				(!queryParam.AverageStar.HasValue || x.Average_Star >= queryParam.AverageStar) &&
				(!queryParam.Country.HasValue || x.CountryId == queryParam.Country) &&
				(!queryParam.Category.HasValue || x.MovieCategories.Select(x => x.CategoryId).ToList().Contains(queryParam.Category.Value)) &&
				(!queryParam.ReleaseDate.HasValue || x.Release_Date >= queryParam.ReleaseDate) &&
				(x.IsDelete == false) &&
				(!queryParam.Participant.HasValue || x.MovieParticipants.Select(x => x.ParticipantId).ToList().Contains(queryParam.Participant.Value))
			);
			Func<IReadOnlyList<Movie>, IOrderedQueryable<Movie>>? orderBy = null;

			if (!string.IsNullOrWhiteSpace(queryParam.SortBy))
			{
				orderBy = movies =>
				{
					var query = movies.AsQueryable();
					return queryParam.SortBy.ToLower() switch
					{
						"name" => queryParam.Ascending
							? query.OrderBy(x => x.Name)
							: query.OrderByDescending(x => x.Name),
						"releasedate" => queryParam.Ascending
							? query.OrderBy(x => x.Release_Date)
							: query.OrderByDescending(x => x.Release_Date),
						"averagestar" => queryParam.Ascending
							? query.OrderBy(x => x.Average_Star)
							: query.OrderByDescending(x => x.Average_Star),
						"allowingage" => queryParam.Ascending
							? query.OrderBy(x => x.Allowing_Age)
							: query.OrderByDescending(x => x.Allowing_Age),
						_ => query.OrderByDescending(x => x.Release_Date) // default sorting
					};
				};
			}

			var pageResultDomain = await _movieRepository.GetPagedResultAsync(filter, orderBy, queryParam.Includes, queryParam.PageIndex, queryParam.PageSize);
			var pageResultDto = _mapper.Map<PagedResult<MovieDto>>(pageResultDomain);
			return pageResultDto;
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

		public async Task<MovieDto> UpdateMovieAsync(Guid id, UpdateMovieRequestDto updateMovieRequestDto)
		{
			await IsExsited(id);
			var movieDomain = await _movieRepository.GetByIdAsync(id);
			List<MovieCategory> movieCate = new List<MovieCategory>();
			if (updateMovieRequestDto.CategoryIds != null)
			{
				foreach (var category in updateMovieRequestDto.CategoryIds)
				{
					movieCate.Add(new MovieCategory { CategoryId = category, MovieId = id });
				}
			}
			List<MovieParticipant> movieParticipants = new List<MovieParticipant>();
			if (updateMovieRequestDto.ParticipantIds != null)
			{
				foreach (var paticipant in updateMovieRequestDto.ParticipantIds)
				{
					movieParticipants.Add(new MovieParticipant { ParticipantId = paticipant, MovieId = id });
				}
			}
			movieDomain.Name = updateMovieRequestDto.Name != null ? updateMovieRequestDto.Name : movieDomain.Name;
			movieDomain.Description = updateMovieRequestDto.Description != null ? updateMovieRequestDto.Description : movieDomain.Description;
			if (updateMovieRequestDto.ThumbnailImage != null)
			{
				// remove old file
			}
			movieDomain.Thumbnail = updateMovieRequestDto.ThumbnailImage != null ? updateMovieRequestDto.ThumbnailImage.FileName.CreateUrl() : movieDomain.Thumbnail;
			movieDomain.Status = updateMovieRequestDto.Status != null ? updateMovieRequestDto.Status.Value : movieDomain.Status;
			movieDomain.Quality = updateMovieRequestDto.Quality != null ? updateMovieRequestDto.Quality.Value : movieDomain.Quality;
			movieDomain.Movie_Url = updateMovieRequestDto.Movie != null ? updateMovieRequestDto.Movie.FileName.CreateUrl() : movieDomain.Movie_Url;
			if (updateMovieRequestDto.Movie != null)
			{
				// remove old file
			}
			movieDomain.Allowing_Age = updateMovieRequestDto.Allowing_Age != null ? updateMovieRequestDto.Allowing_Age.Value : movieDomain.Allowing_Age;
			movieDomain.Release_Date = updateMovieRequestDto.Release_Date != null ? updateMovieRequestDto.Release_Date.Value : movieDomain.Release_Date;
			movieDomain.Duration = updateMovieRequestDto.Duration != null ? updateMovieRequestDto.Duration.Value : movieDomain.Duration;
			movieDomain.IsDelete = updateMovieRequestDto.IsDelete != null ? updateMovieRequestDto.IsDelete.Value : movieDomain.IsDelete;
			movieDomain.CountryId = updateMovieRequestDto.CountryId != null ? updateMovieRequestDto.CountryId.Value : movieDomain.CountryId;
			movieDomain.MovieCategories = movieCate.Count > 0 ? movieCate : movieDomain.MovieCategories;
			movieDomain.MovieParticipants = movieParticipants.Count > 0 ? movieParticipants : movieDomain.MovieParticipants;
			var updatedMovieDomain = await _movieRepository.UpdateAsync(movieDomain);
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

		public async Task<MovieDto> SoftDeleteAsync(Guid id)
		{
			await IsExsited(id);
			var movie = await _movieRepository.SoftDelete(id);
			var movieDto = _mapper.Map<MovieDto>(movie);
			return movieDto;
		}
	}
}
