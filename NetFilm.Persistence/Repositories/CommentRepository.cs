using Microsoft.EntityFrameworkCore;
using NetFilm.Domain.Entities;
using NetFilm.Domain.Interfaces;
using NetFilm.Persistence.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Persistence.Repositories
{
	public class CommentRepository : BaseRepository<Comment, Guid>, ICommentRepository
	{
        private readonly NetFilmDbContext _context;
		public CommentRepository(NetFilmDbContext context) : base(context)
		{
            _context = context;
		}

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            return await _context.Comments.Include(c => c.User).Include(c => c.Movie).Where(c => c.IsDelete == false).OrderByDescending(c => c.Date).ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetByIdCommentAsync(Guid commentId)
        {
            return await _context.Comments.Where(c => c.CommentId == commentId && c.IsDelete == false).OrderByDescending(c => c.Date).ToListAsync();
        }


        public async Task<Comment> GetCommentByIdAsync(Guid id)
        {
            return await _context.Comments.Include(c => c.User).Include(c => c.Movie).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Comment>> GetAllRepliesByCommentIdAsync(Guid id)
        {
            return await _context.Comments.Include(c => c.User).Include(c => c.Movie).Where(c => c.CommentId == id && c.IsDelete == false).OrderByDescending(c => c.Date).ToListAsync();
        }

        public async Task<Comment> ReplyCommentAsync(Comment reply)
        {
            await _context.Comments.AddAsync(reply);
            await _context.SaveChangesAsync();
            return reply;
        }

        public async Task<Comment> SoftDeleteAsync(Guid id)
        {
            var entity = await _context.Comments.FindAsync(id);
            entity.IsDelete = true;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsByMovieIdAsync(Guid movieId)
        {
            return await _context.Comments.Include(c => c.User).Include(c => c.Movie).Where(c => c.CommentId == null && c.MovieId == movieId && c.IsDelete == false).OrderByDescending(c => c.Date).ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetCommentPagedResultAsync(int pageSize, int pageIndex, string searchTerm, string sortBy, bool ascending)
        {

            // Start with all users as IQueryable
            IQueryable<Comment> query = _context.Comments.Where(c => c.IsDelete == false).Include(c => c.User).Include(c => c.Movie).AsQueryable();

            // Apply search filters if searchTerm is provided
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var search = searchTerm.Trim().ToLower();
                query = query.Where(u =>
                    u.Content.ToLower().Contains(searchTerm)
                    || u.User.LastName.ToLower().Contains(searchTerm)
                    || u.Movie.Name.ToLower().Contains(searchTerm)
                    || u.Date.ToString().ToLower().Contains(searchTerm)
                );
            }

            // Apply sorting
            query = sortBy?.ToLower() switch
            {
                "content" => ascending
                    ? query.OrderBy(u => u.Content)
                    : query.OrderByDescending(u => u.Content),
                "username" => ascending
                ? query.OrderBy(u => u.User.UserName)
                : query.OrderByDescending(u => u.User.UserName),
                "movie" => ascending
                ? query.OrderBy(u => u.Movie.Name)
                : query.OrderByDescending(u => u.Movie.Name),
                "date" => ascending
                    ? query.OrderBy(u => u.Date)
                    : query.OrderByDescending(u => u.Date),
                _ => query.OrderByDescending(u => u.Date) // default sorting
            };

            // Apply pagination
            var comments = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();


            return comments;
        }
    }
}
