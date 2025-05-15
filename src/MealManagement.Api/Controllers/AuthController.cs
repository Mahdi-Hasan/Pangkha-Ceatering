using MealManagement.Application.DTOs;
using MealManagement.Application.Features.Auth.Commands;
using MealManagement.Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MealManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Login(LoginDto loginDto)
        {
            var query = new LoginQuery
            {
                Identifier = loginDto.Identifier,
                Password = loginDto.Password
            };

            var token = await _mediator.Send(query);
            return Ok(token);
        }

        [HttpPost("request-otp")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> RequestOtp(RequestOtpDto requestOtpDto)
        {
            var command = new RequestOtpCommand
            {
                PhoneNumber = requestOtpDto.PhoneNumber,
                IsForSignup = true
            };

            var verificationId = await _mediator.Send(command);
            return Ok(new { verificationId });
        }

        [HttpPost("verify-otp")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> VerifyOtp(VerifyOtpDto verifyOtpDto)
        {
            var command = new VerifyOtpCommand
            {
                PhoneNumber = verifyOtpDto.PhoneNumber,
                VerificationId = verifyOtpDto.VerificationId,
                Otp = verifyOtpDto.Otp
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("complete-signup")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> CompleteSignup(CompleteSignupDto completeSignupDto)
        {
            var command = new CompleteSignupCommand
            {
                PhoneNumber = completeSignupDto.PhoneNumber,
                Email = completeSignupDto.Email,
                Username = completeSignupDto.Username,
                Password = completeSignupDto.Password,
                Name = completeSignupDto.Name,
                Address = completeSignupDto.Address
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("request-password-reset")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> RequestPasswordReset(RequestOtpDto requestOtpDto)
        {
            var command = new RequestOtpCommand
            {
                PhoneNumber = requestOtpDto.PhoneNumber,
                IsForSignup = false
            };

            var verificationId = await _mediator.Send(command);
            return Ok(new { verificationId });
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            // First verify the OTP
            var verifyCommand = new VerifyOtpCommand
            {
                PhoneNumber = resetPasswordDto.PhoneNumber,
                VerificationId = resetPasswordDto.VerificationId,
                Otp = resetPasswordDto.Otp
            };

            var isOtpValid = await _mediator.Send(verifyCommand);
            
            if (!isOtpValid)
            {
                return BadRequest("Invalid OTP");
            }

            // Then reset the password
            var resetCommand = new ResetPasswordCommand
            {
                PhoneNumber = resetPasswordDto.PhoneNumber,
                NewPassword = resetPasswordDto.NewPassword
            };

            var result = await _mediator.Send(resetCommand);
            return Ok(result);
        }
    }
}