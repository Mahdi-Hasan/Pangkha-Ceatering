using MealManagement.Domain.Enums;
using System;

namespace MealManagement.Application.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
        public UserRole Role { get; set; }
    }
}