using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_Health_Consultation_Portal.Application.Commands.Users;
using Online_Health_Consultation_Portal.Application.Dtos.Users;
using Online_Health_Consultation_Portal.Application.Queries.Users;

namespace Online_Health_Consultation_Portal.API.Controllers.User
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var query = new GetUserProfileQuery { User = User };
                var result = await _mediator.Send(query);

                if (result == null)
                {
                    return NotFound("User profile not found.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("profile")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateUserProfileDto profile, [FromQuery] int? userId = null)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var command = new UpdateUserProfileCommand
                {
                    User = User,
                    TargetUserId = userId,
                    Profile = profile
                };

                var result = await _mediator.Send(command);

                if (result)
                    return NoContent(); // 204 on success

                return Conflict("Failed to update profile. The data may be out of date or invalid.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "Update failed",
                    details = ex.Message
                });
            }
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            try
            {
                if (dto == null || string.IsNullOrEmpty(dto.CurrentPassword) || string.IsNullOrEmpty(dto.NewPassword))
                {
                    return BadRequest("Current and new password must be provided.");
                }

                var command = new ChangePasswordCommand
                {
                    User = User,
                    changePasswordDto = dto
                };

                var result = await _mediator.Send(command);

                if (!result)
                    return BadRequest("Incorrect current password or failed to update.");

                return Ok("Password changed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{userId}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                await _mediator.Send(new DeleteUserCommand { UserId = userId });
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var user = await _mediator.Send(new GetUserByIdQuery { UserId = userId });

            if (user == null)
                return NotFound($"User with id {userId} not found.");

            return Ok(user);
        }


        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetUsers([FromQuery] GetUsersQuery query)
        {
            try
            {
            var result = await _mediator.Send(query);
            return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}