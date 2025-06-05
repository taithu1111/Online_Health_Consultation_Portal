using MediatR;
using Online_Health_Consultation_Portal.Application.Commands.Auth;
using Online_Health_Consultation_Portal.Application.Dtos.Auth.LoginDto;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Infrastructure.Repositories;
using Online_Health_Consultation_Portal.Infrastructure.Service;
using Microsoft.AspNetCore.Identity;

namespace Online_Health_Consultation_Portal.Application.Handlers.Auth
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly UserManager<User> _userManager;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IJwtService jwtService,
            UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _userManager = userManager;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.LoginDto.Email);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password.");

            var passwordValid = await _userManager.CheckPasswordAsync(user, request.LoginDto.Password);
            if (!passwordValid)
                throw new UnauthorizedAccessException("Invalid email or password.");

            // Wrap the single Role string as a list
            var roles = !string.IsNullOrEmpty(user.Role)
                ? new List<string> { user.Role }
                : new List<string>();

            var token = _jwtService.GenerateToken(user.Id, roles);

            return new LoginResponseDto
            {
                UserId = user.Id,
                Token = token,
                Expires = DateTime.UtcNow.AddHours(1),
                Roles = roles
            };
        }
    }
}
