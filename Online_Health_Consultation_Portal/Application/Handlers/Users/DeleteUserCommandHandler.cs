using MediatR;
using Online_Health_Consultation_Portal.Application.Commands.Users;
using Online_Health_Consultation_Portal.Infrastructure.Services;

public sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserManagementService _userService;
    private readonly ILogger<DeleteUserCommandHandler> _logger;

    public DeleteUserCommandHandler(
        IUserManagementService userService,
        ILogger<DeleteUserCommandHandler> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _userService.PermanentlyDeleteUserAsync(request.UserId);
        
        _logger.LogWarning($"Admin deleted user {request.UserId}");

        return Unit.Value;
    }
}