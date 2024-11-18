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

		public async Task<MovieDetailDto> AddMovieAsync(string movieName, string movieUrl)
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
				Comments = []
			};
			var createdMovieDomain = await _movieRepository.AddAsync(movieDomain);
			var createdMovieDto = _mapper.Map<MovieDetailDto>(createdMovieDomain);
			return createdMovieDto;
		}

		public async Task<IEnumerable<MovieDto>> GetAllAsync()
		{
			var movieDomains = await _movieRepository.GetAllAsync();
			var movieDtos = _mapper.Map<IEnumerable<MovieDto>>(movieDomains);
			return movieDtos;
		}

		public async Task<MovieDetailDto> GetByIdAsync(Guid id)
		{
			var isExisted = await _movieRepository.ExistsAsync(id);
			if (!isExisted)
			{
				throw new NotFoundException($"Can not found movie with id: {id}");
			}
			var movieDomain = await _movieRepository.GetByIdAsync(id);
			var movieDto = _mapper.Map<MovieDetailDto>(movieDomain);
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

		public async Task<PagedResult<MovieViewerDto>> GetMoviePaging(MovieQueryParam queryParam)
		{
			// Define the filter expression
			Expression<Func<Movie, bool>> filter = x => (
				(string.IsNullOrWhiteSpace(queryParam.SearchTerm) || x.Name.Contains(queryParam.SearchTerm) || x.Description.Contains(queryParam.SearchTerm)) &&
				(!queryParam.Status.HasValue || x.Status == queryParam.Status) &&
				(!queryParam.Quality.HasValue || x.Quality == queryParam.Quality) &&
				(!queryParam.AllowingAge.HasValue || x.Allowing_Age >= queryParam.AllowingAge) &&
				(!queryParam.AverageStar.HasValue || x.Average_Star >= queryParam.AverageStar) &&
				(!queryParam.Country.HasValue || x.CountryId == queryParam.Country) &&
				(!queryParam.Category.HasValue || x.MovieCategories.Select(mc => mc.CategoryId).Contains(queryParam.Category.Value)) &&
				(!queryParam.ReleaseDate.HasValue || x.Release_Date >= queryParam.ReleaseDate) &&
				(x.IsDelete == false) &&
				(!queryParam.Participant.HasValue || x.MovieParticipants.Select(mp => mp.ParticipantId).Contains(queryParam.Participant.Value))
			);

			// Define the ordering function
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
						"totalviews" => queryParam.Ascending
							? query.OrderBy(x => x.TotalViews)
							: query.OrderByDescending(x => x.TotalViews),
						"allowingage" => queryParam.Ascending
							? query.OrderBy(x => x.Allowing_Age)
							: query.OrderByDescending(x => x.Allowing_Age),
						_ => query.OrderByDescending(x => x.Release_Date)
					};
				};
			}

			var pageResultDomain = await _movieRepository.GetPagedResultAsync(
				filter,
				orderBy,
				queryParam.Includes,
				queryParam.PageIndex,
				queryParam.PageSize);

			var pageResultDto = _mapper.Map<PagedResult<MovieViewerDto>>(pageResultDomain);
			return pageResultDto;
		}

		public async Task<MovieDetailDto> UpdateMovieAsync(Guid id, AddMovieRequestDto addMovieRequestDto)
		{
			await IsExsited(id);
			var movieDomain = _mapper.Map<Movie>(addMovieRequestDto);
			List<MovieCategory> movieCate = new List<MovieCategory>();
			foreach (var category in addMovieRequestDto.CategoryIds.Split(","))
			{
				movieCate.Add(new MovieCategory { CategoryId = Guid.Parse(category), MovieId = id });
			}
			List<MovieParticipant> movieParticipants = new List<MovieParticipant>();
			foreach (var participant in addMovieRequestDto.ParticipantIds.Split(","))
			{
				movieParticipants.Add(new MovieParticipant { ParticipantId = Guid.Parse(participant), MovieId = id });
			}
			movieDomain.MovieCategories = movieCate;
			movieDomain.MovieParticipants = movieParticipants;
			var updatedMovieDomain = await _movieRepository.UpdateDetails(id, movieDomain);
			var updatedMovieDto = _mapper.Map<MovieDetailDto>(updatedMovieDomain);
			return updatedMovieDto;
		}

		public async Task<MovieDetailDto> UpdateThumbnailAsync(Guid id, string thumbnail)
		{
			await IsExsited(id);
			var updatedMovieDomain = await _movieRepository.UpddateThumbnail(id, thumbnail);
			var updatedMovieDto = _mapper.Map<MovieDetailDto>(updatedMovieDomain);
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

		public async Task<MovieDto> AddView(Guid id)
		{
			await IsExsited(id);
			var movie = await _movieRepository.GetByIdAsync(id);
			++movie.TotalViews;
			var updatedMovie = await _movieRepository.UpdateAsync(movie);
			return _mapper.Map<MovieDto>(updatedMovie);
		}

		public async Task<MovieDetailDto> UpdateMovieDetails(MovieDetailDto movie)
		{
			var movieDomain = _mapper.Map<Movie>(movie);
			await _movieRepository.UpdateNewAsync(movie.Id, movieDomain);
			return movie;
		}
	}
}
