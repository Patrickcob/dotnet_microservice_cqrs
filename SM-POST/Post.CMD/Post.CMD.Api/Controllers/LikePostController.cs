using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.CMD.Api.Commands;
using Post.Common.DTOs;

namespace Post.CMD.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LikePostController : ControllerBase
    {
        private readonly ILogger<LikePostController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public LikePostController(ILogger<LikePostController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> LikePostAsync(Guid id)
        {
            try
            {
                await _commandDispatcher.SendAsync(new LikePostCommand { Id = id });

                return Ok(new BaseResponse { Message = "Liked post successfully" });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Bad request - {Message}", ex.Message);
                return BadRequest(new BaseResponse { Message = ex.Message });
            }
            catch (AggregateNotFoundException ex)
            {
                var message = $"Could not retrieve aggregate of id:{id.ToString()}";
                _logger.LogError(ex, message , ex.Message);
                return BadRequest(new BaseResponse { Message = message });
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