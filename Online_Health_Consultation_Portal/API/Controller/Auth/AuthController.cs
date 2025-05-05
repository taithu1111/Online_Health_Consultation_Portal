using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Online_Health_Consultation_Portal.Application.Commands.Auth;
using Online_Health_Consultation_Portal.Application.Dtos.Auth.LoginDto;
using Online_Health_Consultation_Portal.Application.Dtos.Auth;

namespace Online_Health_Consultation_Portal.API.Controller.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginDto loginDto)
        {
            var command = new LoginCommand { LoginDto = loginDto };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        /// <summary>
        /// Đăng xuất (xóa token phía client).
        /// </summary>
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Đăng xuất phía client bằng cách xóa token
            return Ok(new { message = "Logout successful." });
        }

        /// <summary>
        /// Yêu cầu reset mật khẩu và nhận token qua email.
        /// </summary>
        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var command = new ForgotPasswordCommand { ForgotPasswordDto = forgotPasswordDto };
            var result = await _mediator.Send(command);

            if (!result)
            {
                return BadRequest(new { message = "Email not found." });
            }

            return Ok(new { message = "Reset password token sent to your email." });
        }

        /// <summary>
        /// Đặt lại mật khẩu bằng token.
        /// </summary>
        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var command = new ResetPasswordCommand { ResetPasswordDto = resetPasswordDto };
            var result = await _mediator.Send(command);

            if (!result)
            {
                return BadRequest(new { message = "Invalid token or token expired." });
            }

            return Ok(new { message = "Password reset successful." });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var command = new RegisterCommand(dto);
            var result = await _mediator.Send(command);

            if (result)
                return Ok("User registered successfully.");
            else
                return BadRequest("Registration failed.");
        }
    }
}
