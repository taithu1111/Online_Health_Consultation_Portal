using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_Health_Consultation_Portal.Application.Commands.Admin.Users;
using Online_Health_Consultation_Portal.Application.Dtos.Admin.Users;
using Online_Health_Consultation_Portal.Application.Dtos.Paginated;

[Route("api/admin/users")]
[Authorize(Policy = "AdminOnly")]
[ApiController]
public class AdminUsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminUsersController(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        await _mediator.Send(new DeleteUserCommand { UserId = userId });
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] GetUsersQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}