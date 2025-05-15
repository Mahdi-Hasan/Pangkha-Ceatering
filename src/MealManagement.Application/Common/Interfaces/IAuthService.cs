using MealManagement.Application.DTOs;
using System.Threading.Tasks;

namespace MealManagement.Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResultDto> AuthenticateAsync(string identifier, string password);
        Task<bool> ValidateTokenAsync(string token);
        Task<UserDto> GetUserFromTokenAsync(string token);
        Task<AuthResultDto> GenerateAuthResultForUserAsync(UserDto user);
    }
}