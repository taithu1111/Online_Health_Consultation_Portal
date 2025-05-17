using MediatR;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Commands.Auth;
using Online_Health_Consultation_Portal.Application.Dtos.Auth.LoginDto;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Infrastructure.Repository;
using Online_Health_Consultation_Portal.Infrastructure.Service;
using System.Text;

namespace Online_Health_Consultation_Portal.Application.Handlers.Auth
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IJwtService _jwtService;

        public LoginCommandHandler(IRepository<User> userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.Get();
            var userInclude = await user
                //.Include(e => e.UserRoles)
                //.ThenInclude(er => er.Role)
                .FirstOrDefaultAsync(e => e.Email == request.LoginDto.Email);

            //var userEmailCheck = await user.FirstOrDefaultAsync(e => e.Email == request.LoginDto.Email);


            if (userInclude == null  /*||!BCrypt.Net.BCrypt.Verify(request.LoginDto.Password, Encoding.UTF8.GetString(userInclude.PasswordHash))*/)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            List<String> roles = userInclude.Role.Split(',').ToList();
            var token = _jwtService.GenerateToken(userInclude.Id, roles);

            return new LoginResponseDto
            {
                Token = token,
                Expires = DateTime.UtcNow.AddHours(1), // Token hết hạn sau 1 giờ
                Roles = roles
            };
        }
    }
}
