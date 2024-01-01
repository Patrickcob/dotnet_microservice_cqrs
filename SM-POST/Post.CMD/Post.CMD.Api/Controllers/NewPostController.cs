using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.DTOs;
using Post.CMD.Api.Commands;

namespace Post.CMD.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class NewPostController : ControllerBase
    {
        private readonly ILogger<NewPostController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public NewPostController(ILogger<NewPostController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> NewPostAsync(NewPostCommand command)
        {
            var id = Guid.NewGuid();
            try
            {
                command.Id = id;

                await _commandDispatcher.SendAsync(command);

                return StatusCode(201, new NewPostResponse { Id = id, Message = "Post created successfully" });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Bad request - {Message}", ex.Message);
                return StatusCode(400, new NewPostResponse { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error - {Message}", ex.Message);
                return StatusCode(500, new NewPostResponse 
                { 
                    Id = id, 
                    Message = ex.Message 
                });
            }
        }
    }
}