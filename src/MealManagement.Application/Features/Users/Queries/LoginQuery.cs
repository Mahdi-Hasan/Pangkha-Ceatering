using MediatR;
using MealManagement.Application.Common.Interfaces;
using MealManagement.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MealManagement.Application.Features.Users.Queries
{
    public class LoginQuery : IRequest<string>
    {
        public string Identifier { get; set; } // Phone, Email, or Username
        public string Password { get; set; }
    }

    public class LoginQueryHandler : IRequestHandler<LoginQuery, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginQueryHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<string> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            // Try to find user by phone, email, or username
            var user = await _userRepository.GetByPhoneAsync(request.Identifier);
            
            if (user == null)
            {
                user = await _userRepository.GetByEmailAsync(request.Identifier);
            }
            
            if (user == null)
            {
                user = await _userRepository.GetByUsernameAsync(request.Identifier);
            }

            if (user == null)
            {
                throw new Exception("Invalid credentials");
            }

            // Check if user is active
            if (!user.IsActive)
            {
                throw new Exception("User account is inactive");
            }

            // Verify password
            if (!_passwordHasher.VerifyPassword(request.Password, user.Password))
            {
                throw new Exception("Invalid credentials");
            }

            // Generate JWT token
            return _jwtTokenGenerator.GenerateToken(user);
        }
    }
}