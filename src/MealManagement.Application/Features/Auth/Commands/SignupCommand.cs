using MealManagement.Application.Common.Extensions;
using MealManagement.Application.Common.Interfaces;
using MealManagement.Application.DTOs;
using MealManagement.Domain.Entities;
using MealManagement.Domain.Enums;
using MealManagement.Domain.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MealManagement.Application.Features.Auth.Commands
{
    public class SignupCommand : IRequest<AuthResultDto>
    {
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class SignupCommandHandler : IRequestHandler<SignupCommand, AuthResultDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAuthService _authService;

        public SignupCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IAuthService authService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _authService = authService;
        }

        public async Task<AuthResultDto> Handle(SignupCommand request, CancellationToken cancellationToken)
        {
            // Check if phone already exists
            if (await _userRepository.PhoneExistsAsync(request.Phone))
            {
                throw new Exception("Phone number already registered");
            }

            // Check if email already exists
            if (!string.IsNullOrEmpty(request.Email) && await _userRepository.EmailExistsAsync(request.Email))
            {
                throw new Exception("Email already registered");
            }

            // Check if username already exists
            if (!string.IsNullOrEmpty(request.Username) && await _userRepository.UsernameExistsAsync(request.Username))
            {
                throw new Exception("Username already taken");
            }

            // Create new user
            var user = new User
            {
                Id = Guid.NewGuid(),
                Phone = request.Phone,
                Email = request.Email,
                Username = request.Username,
                Password = _passwordHasher.HashPassword(request.Password),
                Name = request.Name,
                Address = request.Address,
                Balance = 0,
                IsActive = true,
                Role = UserRole.Customer
            };

            await _userRepository.AddAsync(user);

            // Generate auth result
            var userDto = user.ToDto();
            return await _authService.GenerateAuthResultForUserAsync(userDto);
        }
    }
}