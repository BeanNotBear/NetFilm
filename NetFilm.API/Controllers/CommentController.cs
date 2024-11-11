using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetFilm.Application.Attributes;
using NetFilm.Application.DTOs.CategoryDtos;
using NetFilm.Application.DTOs.CommentDTOs;
using NetFilm.Application.Interfaces;

namespace NetFilm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            var comments = await _commentService.GetAll();
            return Ok(comments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById(Guid id)
        {
            var comment = await _commentService.GetById(id);
            return Ok(comment);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> AddComment(AddCommentDto addCommentDto)
        {
            var comment = await _commentService.Add(addCommentDto);
            return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, comment);
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateComment(Guid id, UpdateCommentDto updateComment)
        {
            var comment = await _commentService.Update(id, updateComment);
            return Ok(comment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            var comment = await _commentService.SoftDelete(id);
            return Ok(comment);
        }

        [HttpPost("reply")]
        [ValidateModel]
        public async Task<IActionResult> Reply(AddReplyDto addReplyDto)
        {
            var reply = await _commentService.Reply(addReplyDto);
            return Ok(reply);
        }

        [HttpGet("reply/{commentId}")]
        public async Task<IActionResult> GetAllRepliesByCommentId(Guid commentId)
        {
            var comments = await _commentService.GetAllRepliesByCommentId(commentId);
            return Ok(comments);
        }

        [HttpGet("movie/{movieId}")]
        public async Task<IActionResult> GetAllCommentByMovieId(Guid movieId)
        {
            var comments = await _commentService.GetCommentByMovieId(movieId);
            return Ok(comments);
        }
    }
}
