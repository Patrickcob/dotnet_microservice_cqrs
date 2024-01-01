using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.CMD.Api.Commands;
using Post.Common.DTOs;

namespace Post.CMD.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EditCommentController : ControllerBase
    {
        private readonly ILogger<EditCommentController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public EditCommentController(ILogger<EditCommentController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditCommentAsync(Guid id, [FromBody] EditCommentCommand command)
        {
            try
            {
                command.Id = id;
                await _commandDispatcher.SendAsync(command);

                return Ok(new BaseResponse { Message = "Comment edited successfully" });
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