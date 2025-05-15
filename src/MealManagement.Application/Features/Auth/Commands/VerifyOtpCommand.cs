using MealManagement.Application.Common.Interfaces;
using MediatR;

namespace MealManagement.Application.Features.Auth.Commands
{
    public class VerifyOtpCommand : IRequest<bool>
    {
        public string PhoneNumber { get; set; }
        public string VerificationId { get; set; }
        public string Otp { get; set; }
    }

    public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, bool>
    {
        private readonly IFirebaseOtpService _firebaseOtpService;

        public VerifyOtpCommandHandler(IFirebaseOtpService firebaseOtpService)
        {
            _firebaseOtpService = firebaseOtpService;
        }

        public async Task<bool> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            // Verify OTP via Firebase
            var isValid = await _firebaseOtpService.VerifyOtpAsync(request.VerificationId, request.Otp);
            
            if (!isValid)
            {
                throw new Exception("Invalid OTP");
            }
            
            return true;
        }
    }
}