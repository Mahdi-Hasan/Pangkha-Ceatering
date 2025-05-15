using MealManagement.Application.DTOs;
using MealManagement.Application.Features.Users.Commands;
using MealManagement.Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MealManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
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

        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> Signup(SignupDto signupDto)
        {
            var command = new CreateUserCommand
            {
                Phone = signupDto.Phone,
                Email = signupDto.Email,
                Username = signupDto.Username,
                Password = signupDto.Password,
                Name = signupDto.Name,
                Address = signupDto.Address
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            var query = new GetAllUsersQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetUserById(Guid id)
        {
            // Only allow admins or the user themselves to access user details
            var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && currentUserId != id)
            {
                return Forbid();
            }

            var query = new GetUserByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> UpdateUser(Guid id, UpdateUserDto updateUserDto)
        {
            // Only allow admins or the user themselves to update user details
            var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && currentUserId != id)
            {
                return Forbid();
            }

            var command = new UpdateUserCommand
            {
                Id = id,
                Phone = updateUserDto.Phone,
                Email = updateUserDto.Email,
                Username = updateUserDto.Username,
                Name = updateUserDto.Name,
                Address = updateUserDto.Address
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> ToggleUserStatus(Guid id)
        {
            var command = new ToggleUserStatusCommand { Id = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUserProfile()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var query = new GetUserByIdQuery { Id = userId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("firebase-token")]
        [Authorize]
        public async Task<ActionResult<bool>> UpdateFirebaseToken(UpdateFirebaseTokenDto updateFirebaseTokenDto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            var command = new UpdateFirebaseTokenCommand
            {
                UserId = userId,
                FirebaseToken = updateFirebaseTokenDto.FirebaseToken
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}