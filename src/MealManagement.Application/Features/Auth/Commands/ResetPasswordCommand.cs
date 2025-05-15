using MediatR;
using MealManagement.Domain.Repositories;
using MealManagement.Application.Common.Interfaces;

namespace MealManagement.Application.Features.Auth.Commands
{
    public class ResetPasswordCommand : IRequest<bool>
    {
        public string PhoneNumber { get; set; }
        public string NewPassword { get; set; }
    }

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public ResetPasswordCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            // Get user by phone number
            var user = await _userRepository.GetByPhoneAsync(request.PhoneNumber);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Update password
            user.Password = _passwordHasher.HashPassword(request.NewPassword);
            await _userRepository.UpdateAsync(user);
            
            return true;
        }
    }
}