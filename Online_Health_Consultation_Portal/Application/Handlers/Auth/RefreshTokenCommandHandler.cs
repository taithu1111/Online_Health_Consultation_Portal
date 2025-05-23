using MediatR;
using Online_Health_Consultation_Portal.Application.Commands.Auth.RefreshToken;
using Online_Health_Consultation_Portal.Application.Dtos.Auth.RefreshToken;
using Online_Health_Consultation_Portal.Application.Interfaces.Repository;
using Online_Health_Consultation_Portal.Application.Interfaces.Service;
using Online_Health_Consultation_Portal.Infrastructure.Service;

namespace Online_Health_Consultation_Portal.Application.Handlers.Auth
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResponseDto?>
    {
        private readonly IJwtService _jwtService;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenRepository _refreshRepo;
        private readonly IAuthBusinessRule _authRule;

        public RefreshTokenCommandHandler(
            IJwtService jwtService,
            ITokenService tokenService,
            IRefreshTokenRepository refreshRepo,
            IAuthBusinessRule authRule)
        {
            _jwtService = jwtService;
            _tokenService = tokenService;
            _refreshRepo = refreshRepo;
            _authRule = authRule;
        }

        public async Task<TokenResponseDto?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            //var result = await _jwtService.RefreshTokenAsync(request.TokenDto.UserId, request.TokenDto.RefreshToken);
            //if (result == null) return null;

            //return new TokenResponseDto
            //{
            //    AccessToken = result.Value.AccessToken,
            //    RefreshToken = result.Value.RefreshToken
            //};

            var user = await _refreshRepo.GetUserByIdAsync(request.TokenDto.UserId);
            if (user == null || _authRule.IsRefreshTokenExpired(user)) return null;

            var isValid = await _refreshRepo.ValidateRefreshTokenAsync(user, request.TokenDto.RefreshToken);
            if (!isValid) return null;

            var roles = await _refreshRepo.GetUserRolesAsync(user);
            var newAccessToken = _tokenService.GenerateAccessToken(user.Id, roles.ToList());
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            var saved = await _refreshRepo.SaveRefreshTokenAsync(user, newRefreshToken);
            if (!saved) return null;

            return new TokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
    }


}
