using System;

namespace MealManagement.Application.DTOs
{
    public class AuthResponseDto
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}