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
    public class EditMessageController : ControllerBase
    {
        private readonly ILogger<EditMessageController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public EditMessageController(ILogger<EditMessageController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditMessageAsync(Guid id, EditMessageCommand command)
        {
            try
            {
                command.Id = id;

                await _commandDispatcher.SendAsync(command);

                return Ok(new BaseResponse { Message = "Post edited successfully" });
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