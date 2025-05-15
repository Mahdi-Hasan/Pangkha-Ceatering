using MediatR;
using MealManagement.Domain.Repositories;
using MealManagement.Application.Common.Interfaces;

namespace MealManagement.Application.Features.Auth.Commands
{
    public class RequestOtpCommand : IRequest<string>
    {
        public string PhoneNumber { get; set; }
        public bool IsForSignup { get; set; }
    }

    public class RequestOtpCommandHandler : IRequestHandler<RequestOtpCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IFirebaseOtpService _firebaseOtpService;

        public RequestOtpCommandHandler(
            IUserRepository userRepository,
            IFirebaseOtpService firebaseOtpService)
        {
            _userRepository = userRepository;
            _firebaseOtpService = firebaseOtpService;
        }

        public async Task<string> Handle(RequestOtpCommand request, CancellationToken cancellationToken)
        {
            // Check if the phone number exists for password reset
            if (!request.IsForSignup)
            {
                var userExists = await _userRepository.PhoneExistsAsync(request.PhoneNumber);
                if (!userExists)
                {
                    throw new Exception("No account found with this phone number");
                }
            }
            // Check if the phone number already exists for signup
            else
            {
                var userExists = await _userRepository.PhoneExistsAsync(request.PhoneNumber);
                if (userExists)
                {
                    throw new Exception("Phone number already registered");
                }
            }

            // Send OTP via Firebase
            var verificationId = await _firebaseOtpService.SendOtpAsync(request.PhoneNumber);
            
            return verificationId;
        }
    }
}