using AutoMapper;
using NetFilm.Application.DTOs.CategoryDtos;
using NetFilm.Application.DTOs.CommentDTOs;
using NetFilm.Application.DTOs.MovieCategoryDtos;
using NetFilm.Application.Exceptions;
using NetFilm.Application.Interfaces;
using NetFilm.Domain.Common;
using NetFilm.Domain.Entities;
using NetFilm.Domain.Interfaces;
using NetFilm.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Infrastructure.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository commentRepository,IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }
        public async Task<CommentDto> Add(AddCommentDto addCommentDto)
        {
            var comment = _mapper.Map<Comment>(addCommentDto);
            comment.Id = Guid.NewGuid();
            comment.Date = DateTime.Now;
            await _commentRepository.AddAsync(comment);
            return _mapper.Map<CommentDto>(await _commentRepository.GetCommentByIdAsync(comment.Id));
        }

        public async Task<IEnumerable<CommentDto>> GetAll()
        {
            var comments = await _commentRepository.GetAllCommentsAsync();
            return _mapper.Map<IEnumerable<CommentDto>>(comments);
        }

        public async Task<CommentDto> GetById(Guid id)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(id);
            if (comment == null)
            {
                throw new NotFoundException($"Can not found comment with Id {id}");
            }
            return _mapper.Map<CommentDto>(comment) ;
        }

        public async Task<IEnumerable<CommentDto>> GetCommentByMovieId(Guid movieId)
        {
            var comments = await _commentRepository.GetAllCommentsByMovieIdAsync(movieId);
            return _mapper.Map<IEnumerable<CommentDto>>(comments);
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByCommentId(Guid commentId)
        {
            var comments = await _commentRepository.GetByIdCommentAsync(commentId);
            return _mapper.Map<IEnumerable<CommentDto>>(comments);
        }

        public async Task<IEnumerable<ReplyDto>> GetAllRepliesByCommentId(Guid commentId)
        {
            var replies = await _commentRepository.GetAllRepliesByCommentIdAsync(commentId);
            return _mapper.Map<IEnumerable<ReplyDto>>(replies);
        }

        public async Task<bool> HardDelete(Guid id)
        {
            var isExisted = await _commentRepository.ExistsAsync(id);
            if (!isExisted)
            {
                throw new NotFoundException($"Can not found comment with Id {id}");
            }
            var isDeleted =  await _commentRepository.DeleteAsync(id);
            if (!isDeleted)
            {
                throw new Exception("Some things went wrong!");
            }
            return true;
        }

        public async Task<ReplyDto> Reply(AddReplyDto reply)
        {
            var commentExisted = await _commentRepository.GetCommentByIdAsync(reply.CommentId);
            var comment = new Comment
            {
                Id = new Guid(),
                Content = reply.Content,
                Date = DateTime.Now,
                CommentId = reply.CommentId,
                MovieId = commentExisted.MovieId,
                UserId = reply.UserId,
            };
            await _commentRepository.AddAsync(comment);
            return _mapper.Map<ReplyDto>(comment);
        }

        public async Task<CommentDto> SoftDelete(Guid id)
        {
            var isExisted = await _commentRepository.ExistsAsync(id);
            if (!isExisted)
            {
                throw new NotFoundException($"Can not found comment with Id {id}");
            }
            await _commentRepository.SoftDeleteAsync(id);
            return _mapper.Map<CommentDto>(await _commentRepository.GetCommentByIdAsync(id));
        }

        public async Task<CommentDto> Update(Guid id, UpdateCommentDto updateCommentDto)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                throw new NotFoundException($"Can not found category with Id {id}");
            }
            _mapper.Map(updateCommentDto, comment);
            var updateComment = await _commentRepository.UpdateAsync(comment);
            if (updateComment == null)
            {
                throw new Exception("Some things went wrong!");
            }
            return _mapper.Map<CommentDto>(await _commentRepository.GetCommentByIdAsync(id));
        }
        public async Task<PagedResult<CommentDto>> GetCommentPagedResult(CommentQueryParams queryParams)
        {
            // Validate
            queryParams.Validate();

            var comments = await _commentRepository.GetCommentPagedResultAsync(queryParams.PageSize, queryParams.PageIndex, queryParams.SearchTerm, queryParams.SortBy, queryParams.Ascending);
            var commentsDto = _mapper.Map<IEnumerable<CommentDto>>(comments);
            var totalItem = commentsDto.Count();
            return new PagedResult<CommentDto>(commentsDto, totalItem, queryParams.PageIndex, queryParams.PageSize);
        }
    }
}
