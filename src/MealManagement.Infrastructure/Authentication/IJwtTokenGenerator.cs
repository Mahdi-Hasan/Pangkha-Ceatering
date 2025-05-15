using MealManagement.Domain.Entities;
using System;

namespace MealManagement.Infrastructure.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}