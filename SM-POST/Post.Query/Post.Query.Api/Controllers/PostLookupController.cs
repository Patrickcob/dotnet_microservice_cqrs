using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.CMD.Domain.Entities;
using Post.Common.DTOs;
using Post.Query.Api.DTOs;
using Post.Query.Api.Queries;

namespace Post.Query.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PostLookupController : ControllerBase
    {
        private readonly ILogger<PostLookupController> _logger;
        private readonly IQueryDispatcher<PostEntity> _queryDispatcher;

        public PostLookupController(ILogger<PostLookupController> logger, IQueryDispatcher<PostEntity> queryDispatcher)
        {
            _logger = logger;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPostsAsync()
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindAllPostsQuery());

                if (posts == null || !posts.Any())
                {
                    return NoContent();
                }

                return Ok(new PostLookupResponse { Posts = posts, Message = "Posts retrieved successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error - {Message}", ex.Message);
                return StatusCode(500, new BaseResponse
                {
                    Message = ex.Message
                });
            }
        }

        [HttpGet("byId/{postId}")]
        public async Task<IActionResult> GetPostByIdAsync(Guid postId)
        {
            try
            {
                var post = await _queryDispatcher.SendAsync(new FindPostByIdQuery { Id = postId });

                if (post == null || !post.Any())
                {
                    return NotFound(new BaseResponse { Message = $"Post with id:{postId.ToString()} not found" });
                }

                return Ok(new PostLookupResponse { Posts = post, Message = $"Post with id:{postId.ToString()} retrieved successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error - {Message}", ex.Message);
                return StatusCode(500, new BaseResponse
                {
                    Message = ex.Message
                });
            }
        }

        [HttpGet("byAuthor/{author}")]
        public async Task<IActionResult> GetPostByAuthorAsync(string author)
        {
            try
            {
                var post = await _queryDispatcher.SendAsync(new FindPostByAuthorQuery { Author = author });

                if (post == null || !post.Any())
                {
                    return NotFound(new BaseResponse { Message = $"Post with author:{author} not found" });
                }

                return Ok(new PostLookupResponse { Posts = post, Message = $"Post with author:{author} retrieved successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error - {Message}", ex.Message);
                return StatusCode(500, new BaseResponse
                {
                    Message = ex.Message
                });
            }
        }

        [HttpGet("withComments")]
        public async Task<IActionResult> GetPostsWithCommentsAsync()
        {
            try
            {
                var post = await _queryDispatcher.SendAsync(new FindPostsWithCommentsQuery());

                if (post == null || !post.Any())
                {
                    return NoContent();
                }

                return Ok(new PostLookupResponse { Posts = post, Message = "Posts with comments retrieved successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error - {Message}", ex.Message);
                return StatusCode(500, new BaseResponse
                {
                    Message = ex.Message
                });
            }
        }

        [HttpGet("withLikes/{numberOfLikes}")]
        public async Task<IActionResult> GetPostsWithLikesAsync(int numberOfLikes)
        {
            try
            {
                var post = await _queryDispatcher.SendAsync(new FindPostsWithLikesQuery { NumberOfLikes = numberOfLikes });

                if (post == null || !post.Any())
                {
                    return NoContent();
                }

                return Ok(new PostLookupResponse { Posts = post, Message = "Posts with likes retrieved successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error - {Message}", ex.Message);
                return StatusCode(500, new BaseResponse
                {
                    Message = ex.Message
                });
            }

        }
    }
}