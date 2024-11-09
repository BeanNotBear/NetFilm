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
            return await _context.Comments.Include(c => c.User).Include(c => c.Movie).ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetByIdCommentAsync(Guid commentId)
        {
            return await _context.Comments.Where(c => c.CommentId == commentId).ToListAsync();
        }


        public async Task<Comment> GetCommentByIdAsync(Guid id)
        {
            return await _context.Comments.Include(c => c.User).Include(c => c.Movie).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Comment>> GetAllRepliesByCommentIdAsync(Guid id)
        {
            return await _context.Comments.Include(c => c.User).Include(c => c.Movie).Where(c => c.CommentId == id).ToListAsync();
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
            return await _context.Comments.Include(c => c.User).Include(c => c.Movie).Where(c => c.CommentId == null && c.MovieId == movieId).ToListAsync();
        }
    }
}
