using MealManagement.Application.Common.Interfaces;
using MealManagement.Application.DTOs;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MealManagement.Application.Features.Auth.Commands
{
    public class LoginCommand : IRequest<AuthResultDto>
    {
        public string Identifier { get; set; }
        public string Password { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResultDto>
    {
        private readonly IAuthService _authService;

        public LoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<AuthResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await _authService.AuthenticateAsync(request.Identifier, request.Password);
        }
    }
}