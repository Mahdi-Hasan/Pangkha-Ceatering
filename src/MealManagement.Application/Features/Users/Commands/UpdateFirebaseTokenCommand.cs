using MediatR;
using MealManagement.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MealManagement.Application.Features.Users.Commands
{
    public class UpdateFirebaseTokenCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
        public string FirebaseToken { get; set; }
    }

    public class UpdateFirebaseTokenCommandHandler : IRequestHandler<UpdateFirebaseTokenCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public UpdateFirebaseTokenCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(UpdateFirebaseTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.FirebaseToken = request.FirebaseToken;
            await _userRepository.UpdateAsync(user);
            
            return true;
        }
    }
}