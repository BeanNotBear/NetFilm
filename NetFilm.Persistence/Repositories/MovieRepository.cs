using Microsoft.EntityFrameworkCore;
using NetFilm.Domain.Common;
using NetFilm.Domain.Entities;
using NetFilm.Domain.Interfaces;
using NetFilm.Persistence.Data;

namespace NetFilm.Persistence.Repositories
{
	public class MovieRepository : BaseRepository<Movie, Guid>, IMovieRepository
	{
		private readonly NetFilmDbContext _dbContext;
		public MovieRepository(NetFilmDbContext context) : base(context)
		{
			_dbContext = context;
		}

		public async Task<bool> ExistesByName(string name)
		{
			var movie = await _dbContext.Movies.FirstOrDefaultAsync(x => x.Name == name);
			if (movie != null)
			{
				return true;
			}
			return false;
		}

		public override async Task<Movie> GetByIdAsync(Guid id)
		{
			var movie = await _dbContext.Movies
				.Include(m => m.Country)
				.Include(m => m.Subtitles)
				.Include(m => m.MovieParticipants)
				.ThenInclude(p => p.Participant)
				.Include(m => m.MovieCategories)
				.ThenInclude(mc => mc.Category)
				.Include(m => m.Votes)
				.FirstOrDefaultAsync(m => m.Id == id);
			return movie;
		}

		public async Task<Movie> SoftDelete(Guid id)
		{
			var movie = await _dbContext.Movies.FindAsync(id);
			movie.IsDelete = true;
			await _dbContext.SaveChangesAsync();
			return movie;
		}

		public async Task<Movie> UpdateDetails(Guid id, Movie movie)
		{
			var existedMovie = await _dbContext.Movies.FindAsync(id);
			existedMovie.Name = movie.Name;
			existedMovie.Description = movie.Description;
			existedMovie.Quality = movie.Quality;
			existedMovie.Allowing_Age = movie.Allowing_Age;
			existedMovie.Release_Date = movie.Release_Date;
			existedMovie.Duration = movie.Duration;
			existedMovie.CountryId = movie.CountryId;
			existedMovie.MovieCategories = movie.MovieCategories;
			existedMovie.MovieParticipants = movie.MovieParticipants;
			existedMovie.Status = Domain.Common.MovieStatus.Active;
			await _dbContext.SaveChangesAsync();
			return existedMovie;
		}

		public async Task<Movie> UpdateNewAsync(Movie entity)
		{
			await _dbContext.SaveChangesAsync();
			return entity;
		}

		public async Task<Movie> UpdateMovie(Movie movie)
		{
			var existedMovie = await _dbContext.Movies.FindAsync(movie.Id);
			existedMovie.Duration = movie.Duration;
			existedMovie.Movie_Url = movie.Movie_Url;
			await _dbContext.SaveChangesAsync();
			return existedMovie;
		}

		public async Task<Movie> UpddateThumbnail(Guid id, string thumbnail)
		{
			var existedMovie = await _dbContext.Movies.FindAsync(id);
			existedMovie.Thumbnail = thumbnail;
			await _dbContext.SaveChangesAsync();
			return existedMovie;
		}
	}
}
