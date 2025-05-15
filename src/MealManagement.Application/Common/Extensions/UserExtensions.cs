using MealManagement.Application.DTOs;
using MealManagement.Domain.Entities;

namespace MealManagement.Application.Common.Extensions
{
    public static class UserExtensions
    {
        public static UserDto ToDto(this User user)
        {
            if (user == null)
                return null;
                
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Phone = user.Phone,
                Name = user.Name,
                Address = user.Address,
                Balance = user.Balance,
                IsActive = user.IsActive,
                Role = user.Role
            };
        }
    }
}