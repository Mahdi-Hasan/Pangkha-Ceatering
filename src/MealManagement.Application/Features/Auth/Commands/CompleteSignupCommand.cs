using MediatR;
using MealManagement.Application.Common.Interfaces;
using MealManagement.Application.DTOs;
using MealManagement.Domain.Entities;
using MealManagement.Domain.Enums;
using MealManagement.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MealManagement.Application.Features.Auth.Commands
{
    public class CompleteSignupCommand : IRequest<UserDto>
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class CompleteSignupCommandHandler : IRequestHandler<CompleteSignupCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public CompleteSignupCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserDto> Handle(CompleteSignupCommand request, CancellationToken cancellationToken)
        {
            // Check if phone already exists (should not happen if OTP verification was successful)
            if (await _userRepository.PhoneExistsAsync(request.PhoneNumber))
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
                Phone = request.PhoneNumber,
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

            // Return DTO
            return new UserDto
            {
                Id = user.Id,
                Phone = user.Phone,
                Email = user.Email,
                Username = user.Username,
                Name = user.Name,
                Address = user.Address,
                Balance = user.Balance,
                IsActive = user.IsActive,
                Role = user.Role
            };
        }
    }
}