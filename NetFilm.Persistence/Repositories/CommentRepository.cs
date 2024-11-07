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

        public async Task<IEnumerable<Comment>> GetByIdCommentAsync(Guid commentId)
        {
            return await _context.Comments.Where(c => c.CommentId == commentId).ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetByIdMovieAsync(Guid movieId)
        {
            return await _context.Comments.Where(c => c.MovieId == movieId).ToListAsync();
        }

        public async Task<Comment> ReplyCommentAsync(Guid commentId,Comment reply)
        {
            reply.CommentId = commentId;
            await _context.Comments.AddAsync(reply);
            await _context.SaveChangesAsync();
            return reply;
        }
    }
}
