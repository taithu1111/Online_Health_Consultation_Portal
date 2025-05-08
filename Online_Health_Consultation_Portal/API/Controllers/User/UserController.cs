using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_Health_Consultation_Portal.Application.Commands.Users;
using Online_Health_Consultation_Portal.Application.Queries.Users;
using Online_Health_Consultation_Portal.Domain;

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
                // Log the exception if needed
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileCommand command)
        {
            // command.User = User;
            // await _mediator.Send(command);
            // return NoContent();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "10"), // Your user ID
                new Claim(ClaimTypes.Email, "example@gmail.com"),
                new Claim(ClaimTypes.Role, "Patient") // or "Doctor"
            };

            var identity = new ClaimsIdentity(claims, "Test");
            var user = new ClaimsPrincipal(identity);

            command.User = user;

            await _mediator.Send(command);
            return NoContent();
        }
    }
}